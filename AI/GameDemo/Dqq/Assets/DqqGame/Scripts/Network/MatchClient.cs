using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using DqqGame.Combat;
using UnityEngine;
using UnityEngine.Networking;

namespace DqqGame.Network
{
    [Serializable] public sealed class JoinPayload { public string displayName; public int heroId; }
    [Serializable] public sealed class JoinResponse { public string ticketId; public string playerId; public string token; public string status; }
    [Serializable] public sealed class UpgradePayload { public string playerId; public string token; public int round; public string upgradeId; }
    [Serializable] public sealed class ResultPayload { public string playerId; public string token; public int round; public string opponentId; public bool didWin; public string replayChecksum; }
    [Serializable] public sealed class TicketResponse { public string ticketId; public string playerId; public string token; public string status; public int queuePosition; public string matchId; public MatchDto match; }
    [Serializable] public sealed class PlayerDto { public string playerId; public string displayName; public int heroId; public int lives; public bool isBot; public bool isEliminated; public int placement; public string[] upgrades; }
    [Serializable] public sealed class PairingDto { public string pairingId; public string playerAId; public string playerBId; public bool resolved; public string winnerId; }
    [Serializable] public sealed class MatchDto { public string matchId; public string status; public int round; public int seed; public string yourPlayerId; public string yourOpponentId; public PlayerDto[] players; public PairingDto[] pairings; }

    public sealed class MatchSession
    {
        public string TicketId;
        public string PlayerId;
        public string Token;
        public MatchDto Match;
        public bool IsOnline => Match != null;

        public PlayerDto LocalPlayer => FindPlayer(PlayerId);
        public PlayerDto Opponent => Match == null ? null : FindPlayer(Match.yourOpponentId);
        public PairingDto LocalPairing
        {
            get
            {
                if (Match?.pairings == null) return null;
                foreach (PairingDto pairing in Match.pairings)
                    if (pairing.playerAId == PlayerId || pairing.playerBId == PlayerId) return pairing;
                return null;
            }
        }

        public PlayerDto FindPlayer(string id)
        {
            if (Match?.players == null || string.IsNullOrEmpty(id)) return null;
            foreach (PlayerDto player in Match.players)
                if (player.playerId == id) return player;
            return null;
        }

        public BuildState BuildOpponentState()
        {
            return BuildPlayerState(Match?.yourOpponentId);
        }

        public BuildState BuildPlayerState(string playerId)
        {
            PlayerDto player = FindPlayer(playerId);
            BuildState state = new BuildState { HeroId = player?.heroId ?? 1 };
            if (player?.upgrades == null) return state;
            foreach (string id in player.upgrades)
            {
                UpgradeConfig config = GameConfig.Upgrade(id);
                if (config != null) state.Apply(config);
            }
            return state;
        }
    }

    public sealed class MatchClient : MonoBehaviour
    {
        private const string DefaultBaseUrl = "http://127.0.0.1:5077";
        private static string resolvedBaseUrl;

        public static string BaseUrl => resolvedBaseUrl ?? (resolvedBaseUrl = ResolveBaseUrl());
        public MatchSession Session { get; private set; }

        private void Awake()
        {
            UnityEngine.Debug.Log("DQQ match server: " + BaseUrl);
        }

        public IEnumerator Join(int heroId, Action<string> onStatus, Action<MatchSession> onMatched,
            Action<string> onFailed)
        {
            yield return EnsureServerAvailable(onStatus);

            JoinPayload payload = new JoinPayload { displayName = $"玩家{UnityEngine.Random.Range(100, 999)}", heroId = heroId };
            string joinJson = JsonUtility.ToJson(payload);
            UnityWebRequest join = JsonRequest("POST", BaseUrl + "/api/matchmaking/join", joinJson);
            yield return join.SendWebRequest();
            if (join.result != UnityWebRequest.Result.Success)
            {
                onFailed?.Invoke($"无法连接匹配服务器 {BaseUrl}，已切换训练模式");
                yield break;
            }

            JoinResponse created = JsonUtility.FromJson<JoinResponse>(join.downloadHandler.text);
            Session = new MatchSession
            {
                TicketId = created.ticketId,
                PlayerId = created.playerId,
                Token = created.token
            };

            for (int attempt = 0; attempt < 20; attempt++)
            {
                onStatus?.Invoke($"正在搜索对手 · {attempt / 2 + 1}s");
                UnityWebRequest poll = UnityWebRequest.Get(BaseUrl + "/api/matchmaking/tickets/" + Session.TicketId);
                poll.timeout = 3;
                yield return poll.SendWebRequest();
                if (poll.result == UnityWebRequest.Result.Success)
                {
                    TicketResponse ticket = JsonUtility.FromJson<TicketResponse>(poll.downloadHandler.text);
                    if (ticket.status == "matched" && ticket.match != null)
                    {
                        Session.Match = ticket.match;
                        onMatched?.Invoke(Session);
                        yield break;
                    }
                }
                yield return new WaitForSecondsRealtime(.5f);
            }
            onFailed?.Invoke("匹配超时，已切换训练模式");
        }

        public IEnumerator SubmitUpgrade(string upgradeId, Action<MatchDto> onComplete, Action<string> onFailed)
        {
            if (Session?.Match == null) yield break;
            UpgradePayload payload = new UpgradePayload
            {
                playerId = Session.PlayerId,
                token = Session.Token,
                round = Session.Match.round,
                upgradeId = upgradeId
            };
            yield return SendMatchPost("/upgrade", JsonUtility.ToJson(payload), onComplete, onFailed);
        }

        public IEnumerator SubmitResult(bool won, string checksum, Action<MatchDto> onComplete, Action<string> onFailed)
        {
            if (Session?.Match == null) yield break;
            ResultPayload payload = new ResultPayload
            {
                playerId = Session.PlayerId,
                token = Session.Token,
                round = Session.Match.round,
                opponentId = Session.Match.yourOpponentId,
                didWin = won,
                replayChecksum = checksum
            };
            yield return SendMatchPost("/result", JsonUtility.ToJson(payload), onComplete, onFailed);
        }

        public IEnumerator Refresh(Action<MatchDto> onComplete, Action<string> onFailed)
        {
            if (Session?.Match == null) yield break;
            string url = BaseUrl + "/api/matches/" + Session.Match.matchId +
                         "?playerId=" + UnityWebRequest.EscapeURL(Session.PlayerId) +
                         "&token=" + UnityWebRequest.EscapeURL(Session.Token);
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.timeout = 4;
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                onFailed?.Invoke(request.error);
                yield break;
            }
            MatchDto match = JsonUtility.FromJson<MatchDto>(request.downloadHandler.text);
            Session.Match = match;
            onComplete?.Invoke(match);
        }

        private IEnumerator SendMatchPost(string suffix, string json, Action<MatchDto> onComplete, Action<string> onFailed)
        {
            UnityWebRequest request = JsonRequest("POST",
                BaseUrl + "/api/matches/" + Session.Match.matchId + suffix, json);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                onFailed?.Invoke("房间同步失败：" + request.error);
                yield break;
            }
            MatchDto match = JsonUtility.FromJson<MatchDto>(request.downloadHandler.text);
            Session.Match = match;
            onComplete?.Invoke(match);
        }

        private static UnityWebRequest JsonRequest(string method, string url, string json)
        {
            UnityWebRequest request = new UnityWebRequest(url, method);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 4;
            return request;
        }

        private static IEnumerator EnsureServerAvailable(Action<string> onStatus)
        {
            UnityWebRequest health = UnityWebRequest.Get(BaseUrl + "/health");
            health.timeout = 1;
            yield return health.SendWebRequest();
            if (health.result == UnityWebRequest.Result.Success) yield break;

            if (!IsLoopbackServer()) yield break;

            onStatus?.Invoke("正在启动本地匹配服务器…");
            TryLaunchServer();
            for (int attempt = 0; attempt < 12; attempt++)
            {
                yield return new WaitForSecondsRealtime(.25f);
                UnityWebRequest retry = UnityWebRequest.Get(BaseUrl + "/health");
                retry.timeout = 1;
                yield return retry.SendWebRequest();
                if (retry.result == UnityWebRequest.Result.Success) yield break;
            }
        }

        private static string ResolveBaseUrl()
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                const string prefix = "--server-url=";
                if (args[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return NormalizeBaseUrl(args[i].Substring(prefix.Length));
                if (string.Equals(args[i], "--server-url", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                    return NormalizeBaseUrl(args[i + 1]);
            }

            string environmentUrl = Environment.GetEnvironmentVariable("DQQ_MATCH_SERVER_URL");
            if (!string.IsNullOrWhiteSpace(environmentUrl)) return NormalizeBaseUrl(environmentUrl);

            string configPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "server-url.txt"));
            if (File.Exists(configPath))
            {
                string configuredUrl = File.ReadAllText(configPath).Trim();
                if (!string.IsNullOrWhiteSpace(configuredUrl)) return NormalizeBaseUrl(configuredUrl);
            }

            return DefaultBaseUrl;
        }

        private static string NormalizeBaseUrl(string value)
        {
            string candidate = value?.Trim().TrimEnd('/');
            if (Uri.TryCreate(candidate, UriKind.Absolute, out Uri uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                return uri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
            UnityEngine.Debug.LogWarning("Invalid DQQ match server URL; using " + DefaultBaseUrl);
            return DefaultBaseUrl;
        }

        private static bool IsLoopbackServer()
        {
            return Uri.TryCreate(BaseUrl, UriKind.Absolute, out Uri uri) && uri.IsLoopback;
        }

        private static void TryLaunchServer()
        {
            try
            {
                string serverPath;
#if UNITY_EDITOR
                serverPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Builds", "Server", "Dqq.MatchServer.exe"));
#else
                serverPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "Server", "Dqq.MatchServer.exe"));
#endif
                if (!File.Exists(serverPath)) return;
                Process.Start(new ProcessStartInfo
                {
                    FileName = serverPath,
                    WorkingDirectory = Path.GetDirectoryName(serverPath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning("Could not launch local match server: " + exception.Message);
            }
        }
    }
}
