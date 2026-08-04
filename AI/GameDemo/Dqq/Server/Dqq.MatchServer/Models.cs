namespace Dqq.MatchServer;

public sealed record JoinRequest(string? DisplayName, int HeroId);
public sealed record TicketCreated(string TicketId, string PlayerId, string Token, string Status);
public sealed record TicketSnapshot(string TicketId, string PlayerId, string Token, string Status,
    int QueuePosition, string? MatchId, MatchSnapshot? Match);
public sealed record UpgradeRequest(string PlayerId, string Token, int Round, string UpgradeId);
public sealed record RoundResultRequest(string PlayerId, string Token, int Round, string OpponentId,
    bool DidWin, string? ReplayChecksum);

public sealed record PlayerSnapshot(string PlayerId, string DisplayName, int HeroId, int Lives,
    bool IsBot, bool IsEliminated, int Placement, string[] Upgrades);
public sealed record PairingSnapshot(string PairingId, string PlayerAId, string? PlayerBId,
    bool Resolved, string? WinnerId);
public sealed record MatchSnapshot(string MatchId, string Status, int Round, int Seed,
    string YourPlayerId, string? YourOpponentId, PlayerSnapshot[] Players, PairingSnapshot[] Pairings);

internal sealed class QueueEntry
{
    public required string TicketId { get; init; }
    public required string PlayerId { get; init; }
    public required string Token { get; init; }
    public required string DisplayName { get; init; }
    public required int HeroId { get; init; }
    public DateTimeOffset JoinedAt { get; init; }
    public string? MatchId { get; set; }
}

internal sealed class PlayerState
{
    public required string PlayerId { get; init; }
    public required string Token { get; init; }
    public required string DisplayName { get; init; }
    public required int HeroId { get; init; }
    public bool IsBot { get; init; }
    public int Lives { get; set; } = 10;
    public int Placement { get; set; }
    public List<string> Upgrades { get; } = [];
}

internal sealed class PairingState
{
    public required string PairingId { get; init; }
    public required string PlayerAId { get; init; }
    public string? PlayerBId { get; init; }
    public bool Resolved { get; set; }
    public string? WinnerId { get; set; }
}

internal sealed class MatchState
{
    public required string MatchId { get; init; }
    public required int Seed { get; init; }
    public int Round { get; set; } = 1;
    public string Status { get; set; } = "draft";
    public DateTimeOffset DraftStartedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset BattleStartedAt { get; set; }
    public DateTimeOffset? FirstResultAt { get; set; }
    public Dictionary<string, PlayerState> Players { get; } = [];
    public List<PairingState> Pairings { get; set; } = [];
    public HashSet<string> UpgradeReady { get; } = [];
}
