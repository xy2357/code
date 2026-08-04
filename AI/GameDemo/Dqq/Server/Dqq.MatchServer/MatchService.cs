using System.Collections.Concurrent;
using System.Globalization;

namespace Dqq.MatchServer;

public sealed class MatchService : BackgroundService
{
    private static readonly string[] BotNames =
        ["扳手七号", "霜糖", "暴击猫", "余烬零", "幻步机", "星核Beta", "铁壳", "小电弧"];

    private readonly object gate = new();
    private readonly List<QueueEntry> queue = [];
    private readonly ConcurrentDictionary<string, QueueEntry> tickets = new();
    private readonly ConcurrentDictionary<string, MatchState> matches = new();
    private readonly TimeSpan botFillDelay;
    private readonly TimeSpan resultGracePeriod;
    private readonly TimeSpan battleHardTimeout;
    private int botCursor;

    public MatchService(IConfiguration configuration)
    {
        botFillDelay = ReadDuration(configuration, "Matchmaking:BotFillDelaySeconds", 8, 1.5, 30);
        resultGracePeriod = ReadDuration(configuration, "Matchmaking:ResultGraceSeconds", 10, 2, 30);
        battleHardTimeout = ReadDuration(configuration, "Matchmaking:BattleHardTimeoutSeconds", 90, 30, 180);
    }

    public TicketCreated Join(JoinRequest request)
    {
        string playerId = $"p_{Guid.NewGuid():N}";
        QueueEntry entry = new()
        {
            TicketId = $"t_{Guid.NewGuid():N}",
            PlayerId = playerId,
            Token = Convert.ToHexString(Guid.NewGuid().ToByteArray()),
            DisplayName = SanitizeName(request.DisplayName),
            HeroId = request.HeroId,
            JoinedAt = DateTimeOffset.UtcNow
        };
        lock (gate) queue.Add(entry);
        tickets[entry.TicketId] = entry;
        return new TicketCreated(entry.TicketId, entry.PlayerId, entry.Token, "waiting");
    }

    public TicketSnapshot? GetTicket(string ticketId)
    {
        if (!tickets.TryGetValue(ticketId, out QueueEntry? entry)) return null;
        if (entry.MatchId != null && matches.TryGetValue(entry.MatchId, out MatchState? match))
            return new TicketSnapshot(entry.TicketId, entry.PlayerId, entry.Token, "matched", 0,
                entry.MatchId, Snapshot(match, entry.PlayerId));
        int position;
        lock (gate) position = Math.Max(1, queue.FindIndex(item => item.TicketId == ticketId) + 1);
        return new TicketSnapshot(entry.TicketId, entry.PlayerId, entry.Token, "waiting", position, null, null);
    }

    public MatchSnapshot? GetMatch(string matchId, string playerId, string token)
    {
        if (!TryAuthorize(matchId, playerId, token, out MatchState? match)) return null;
        return Snapshot(match!, playerId);
    }

    public MatchSnapshot? SubmitUpgrade(string matchId, UpgradeRequest request)
    {
        if (!TryAuthorize(matchId, request.PlayerId, request.Token, out MatchState? match)) return null;
        lock (match!)
        {
            if (request.Round != match.Round || string.IsNullOrWhiteSpace(request.UpgradeId)) return null;
            PlayerState player = match.Players[request.PlayerId];
            if (!match.UpgradeReady.Add(player.PlayerId)) return Snapshot(match, request.PlayerId);
            player.Upgrades.Add(request.UpgradeId.Trim());
            foreach (PlayerState bot in match.Players.Values.Where(item => item.IsBot && item.Lives > 0))
            {
                if (match.UpgradeReady.Add(bot.PlayerId))
                    bot.Upgrades.Add(BotUpgrade(match.Seed, match.Round, bot.HeroId));
            }
            int aliveCount = match.Players.Values.Count(item => item.Lives > 0);
            if (match.UpgradeReady.Count >= aliveCount) BeginBattle(match);
            return Snapshot(match, request.PlayerId);
        }
    }

    public MatchSnapshot? SubmitRoundResult(string matchId, RoundResultRequest request)
    {
        if (!TryAuthorize(matchId, request.PlayerId, request.Token, out MatchState? match)) return null;
        lock (match!)
        {
            // A second client can submit after the first result has already advanced the room.
            // Treat that request as an idempotent success and return the current snapshot.
            if (request.Round < match.Round ||
                (request.Round == match.Round && match.Status == "completed"))
                return Snapshot(match, request.PlayerId);
            if (request.Round != match.Round || match.Status != "battle") return null;
            PairingState? pairing = match.Pairings.FirstOrDefault(item =>
                (item.PlayerAId == request.PlayerId && item.PlayerBId == request.OpponentId) ||
                (item.PlayerBId == request.PlayerId && item.PlayerAId == request.OpponentId));
            if (pairing == null) return null;
            if (pairing.Resolved) return Snapshot(match, request.PlayerId);
            match.FirstResultAt ??= DateTimeOffset.UtcNow;
            string winnerId = request.DidWin ? request.PlayerId : request.OpponentId;
            ResolvePairing(match, pairing, winnerId);

            if (match.Pairings.All(item => item.Resolved)) AdvanceRound(match);
            return Snapshot(match, request.PlayerId);
        }
    }

    public object Stats()
    {
        int queued;
        lock (gate) queued = queue.Count;
        return new
        {
            queued,
            matches = matches.Count,
            botFillDelaySeconds = botFillDelay.TotalSeconds,
            resultGraceSeconds = resultGracePeriod.TotalSeconds,
            battleHardTimeoutSeconds = battleHardTimeout.TotalSeconds,
            utc = DateTimeOffset.UtcNow
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            TryCreateMatches();
            AutoCompleteExpiredDrafts();
            AutoCompleteExpiredBattles();
            await Task.Delay(250, stoppingToken);
        }
    }

    private void AutoCompleteExpiredDrafts()
    {
        foreach (MatchState match in matches.Values)
        {
            lock (match)
            {
                if (match.Status != "draft" || DateTimeOffset.UtcNow - match.DraftStartedAt < TimeSpan.FromSeconds(12))
                    continue;
                foreach (PlayerState player in match.Players.Values.Where(item => item.Lives > 0))
                {
                    if (!match.UpgradeReady.Add(player.PlayerId)) continue;
                    player.Upgrades.Add(BotUpgrade(match.Seed, match.Round, player.HeroId));
                }
                BeginBattle(match);
            }
        }
    }

    private void AutoCompleteExpiredBattles()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        foreach (MatchState match in matches.Values)
        {
            lock (match)
            {
                if (match.Status != "battle") continue;
                bool graceExpired = match.FirstResultAt is { } firstResultAt &&
                                    now - firstResultAt >= resultGracePeriod;
                bool hardTimeoutExpired = now - match.BattleStartedAt >= battleHardTimeout;
                if (!graceExpired && !hardTimeoutExpired) continue;

                foreach (PairingState pairing in match.Pairings.Where(item => !item.Resolved))
                    ResolvePairing(match, pairing, FallbackWinner(match, pairing));
                if (match.Pairings.All(item => item.Resolved)) AdvanceRound(match);
            }
        }
    }

    private void TryCreateMatches()
    {
        lock (gate)
        {
            while (queue.Count >= 4) CreateMatch(queue.Take(4).ToList());
            if (queue.Count > 0 && DateTimeOffset.UtcNow - queue[0].JoinedAt >= botFillDelay)
                CreateMatch(queue.Take(Math.Min(4, queue.Count)).ToList());
        }
    }

    private void CreateMatch(List<QueueEntry> humans)
    {
        foreach (QueueEntry human in humans) queue.Remove(human);
        MatchState match = new()
        {
            MatchId = $"m_{Guid.NewGuid():N}",
            Seed = Random.Shared.Next(100000, 999999)
        };
        foreach (QueueEntry human in humans)
        {
            human.MatchId = match.MatchId;
            match.Players[human.PlayerId] = new PlayerState
            {
                PlayerId = human.PlayerId,
                Token = human.Token,
                DisplayName = human.DisplayName,
                HeroId = human.HeroId,
                IsBot = false
            };
        }
        while (match.Players.Count < 4)
        {
            int index = botCursor++;
            string id = $"bot_{Guid.NewGuid():N}";
            match.Players[id] = new PlayerState
            {
                PlayerId = id,
                Token = string.Empty,
                DisplayName = BotNames[index % BotNames.Length],
                HeroId = 1 + index % 6,
                IsBot = true
            };
        }
        match.Pairings = CreatePairings(match);
        matches[match.MatchId] = match;
    }

    private static List<PairingState> CreatePairings(MatchState match)
    {
        List<PlayerState> alive = match.Players.Values.Where(player => player.Lives > 0)
            .OrderBy(player => StableOrder(match.Seed, match.Round, player.PlayerId)).ToList();
        List<PairingState> pairings = [];
        for (int i = 0; i < alive.Count; i += 2)
        {
            PairingState pairing = new()
            {
                PairingId = $"r{match.Round}_{i / 2}",
                PlayerAId = alive[i].PlayerId,
                PlayerBId = i + 1 < alive.Count ? alive[i + 1].PlayerId : null
            };
            if (pairing.PlayerBId == null)
            {
                pairing.Resolved = true;
                pairing.WinnerId = pairing.PlayerAId;
            }
            pairings.Add(pairing);
        }
        return pairings;
    }

    private static void ResolveBotOnlyPairings(MatchState match)
    {
        foreach (PairingState pairing in match.Pairings.Where(item => !item.Resolved && item.PlayerBId != null))
        {
            PlayerState a = match.Players[pairing.PlayerAId];
            PlayerState b = match.Players[pairing.PlayerBId!];
            if (!a.IsBot || !b.IsBot) continue;
            ResolvePairing(match, pairing, FallbackWinner(match, pairing));
        }
    }

    private static string FallbackWinner(MatchState match, PairingState pairing)
    {
        if (pairing.PlayerBId == null) return pairing.PlayerAId;
        PlayerState a = match.Players[pairing.PlayerAId];
        PlayerState b = match.Players[pairing.PlayerBId];
        int aScore = a.Upgrades.Count * 17 + a.HeroId * 7 + StableOrder(match.Seed, match.Round, a.PlayerId) % 31;
        int bScore = b.Upgrades.Count * 17 + b.HeroId * 7 + StableOrder(match.Seed, match.Round, b.PlayerId) % 31;
        return aScore >= bScore ? a.PlayerId : b.PlayerId;
    }

    private static void BeginBattle(MatchState match)
    {
        if (match.Status == "battle") return;
        match.Status = "battle";
        match.BattleStartedAt = DateTimeOffset.UtcNow;
        match.FirstResultAt = null;
        ResolveBotOnlyPairings(match);
    }

    private static void ResolvePairing(MatchState match, PairingState pairing, string winnerId)
    {
        pairing.Resolved = true;
        pairing.WinnerId = winnerId;
        string loserId = pairing.PlayerAId == winnerId ? pairing.PlayerBId! : pairing.PlayerAId;
        PlayerState loser = match.Players[loserId];
        loser.Lives = Math.Max(0, loser.Lives - 1);
    }

    private static void AdvanceRound(MatchState match)
    {
        List<PlayerState> alive = match.Players.Values.Where(player => player.Lives > 0).ToList();
        if (alive.Count <= 1)
        {
            match.Status = "completed";
            if (alive.Count == 1) alive[0].Placement = 1;
            int placement = 2;
            foreach (PlayerState player in match.Players.Values.Where(player => player.Lives <= 0)
                         .OrderByDescending(player => player.Upgrades.Count))
                player.Placement = placement++;
            return;
        }

        match.Round++;
        match.Status = "draft";
        match.DraftStartedAt = DateTimeOffset.UtcNow;
        match.FirstResultAt = null;
        match.UpgradeReady.Clear();
        match.Pairings = CreatePairings(match);
    }

    private static TimeSpan ReadDuration(IConfiguration configuration, string key, double fallback,
        double minimum, double maximum)
    {
        if (!double.TryParse(configuration[key], NumberStyles.Float, CultureInfo.InvariantCulture,
                out double seconds))
            seconds = fallback;
        return TimeSpan.FromSeconds(Math.Clamp(seconds, minimum, maximum));
    }

    private static MatchSnapshot Snapshot(MatchState match, string playerId)
    {
        PairingState? pairing = match.Pairings.FirstOrDefault(item =>
            item.PlayerAId == playerId || item.PlayerBId == playerId);
        string? opponentId = pairing == null ? null :
            pairing.PlayerAId == playerId ? pairing.PlayerBId : pairing.PlayerAId;
        return new MatchSnapshot(match.MatchId, match.Status, match.Round, match.Seed, playerId, opponentId,
            match.Players.Values.Select(player => new PlayerSnapshot(player.PlayerId, player.DisplayName,
                player.HeroId, player.Lives, player.IsBot, player.Lives <= 0, player.Placement,
                player.Upgrades.ToArray())).ToArray(),
            match.Pairings.Select(item => new PairingSnapshot(item.PairingId, item.PlayerAId,
                item.PlayerBId, item.Resolved, item.WinnerId)).ToArray());
    }

    private bool TryAuthorize(string matchId, string playerId, string token, out MatchState? match)
    {
        if (!matches.TryGetValue(matchId, out match)) return false;
        return match.Players.TryGetValue(playerId, out PlayerState? player) && !player.IsBot &&
               CryptographicEquals(player.Token, token);
    }

    private static bool CryptographicEquals(string left, string right)
    {
        byte[] leftBytes = System.Text.Encoding.UTF8.GetBytes(left);
        byte[] rightBytes = System.Text.Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length &&
               System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }

    private static string SanitizeName(string? name)
    {
        string value = string.IsNullOrWhiteSpace(name) ? $"玩家{Random.Shared.Next(100, 999)}" : name.Trim();
        return value.Length > 16 ? value[..16] : value;
    }

    private static int StableOrder(int seed, int round, string id)
    {
        unchecked
        {
            int hash = seed * 397 ^ round;
            foreach (char character in id) hash = hash * 31 + character;
            return hash & int.MaxValue;
        }
    }

    private static string BotUpgrade(int seed, int round, int heroId)
    {
        string[][] choices =
        [
            ["basic_power", "basic_haste", "basic_force", "basic_combo", "basic_thunder", "basic_sustain"],
            ["crit_rate", "crit_damage", "crit_edge", "crit_tempo", "crit_chase", "crit_apex"],
            ["ult_charge", "ult_power", "ult_body", "ult_focus", "ult_echo", "ult_engine"],
            ["dodge_phase", "dodge_haste", "dodge_shell", "dodge_counter_power", "dodge_counter", "dodge_paradox"],
            ["frost_core", "frost_armor", "frost_tempo", "frost_force", "frost_blade", "frost_crown"],
            ["burn_core", "burn_force", "burn_haste", "burn_vitality", "burn_mark", "burn_inferno"]
        ];
        return choices[heroId - 1][Math.Abs(seed + round + heroId) % choices[heroId - 1].Length];
    }
}
