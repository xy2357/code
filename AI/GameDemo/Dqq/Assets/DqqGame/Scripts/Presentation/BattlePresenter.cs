using System;
using System.Collections;
using System.Collections.Generic;
using DqqGame.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace DqqGame.Presentation
{
    public sealed class BattlePresenter : MonoBehaviour
    {
        private FighterView player;
        private FighterView enemy;
        private RectTransform overlayLayer;
        private Text abilityBanner;
        private Text clockText;
        private Text logText;
        private ScrollRect logScroll;
        private readonly Queue<string> logLines = new Queue<string>();
        private Coroutine playback;
        private Coroutine scrollRoutine;

        public void Initialize(FighterView playerView, FighterView enemyView, RectTransform overlays,
            Text banner, Text clock, Text battleLog, ScrollRect battleLogScroll)
        {
            player = playerView;
            enemy = enemyView;
            overlayLayer = overlays;
            abilityBanner = banner;
            clockText = clock;
            logText = battleLog;
            logScroll = battleLogScroll;
            abilityBanner.text = string.Empty;
        }

        public void ResetHistory()
        {
            logLines.Clear();
            logText.text = "战斗日志将在开战后记录；可用滚轮或拖动右侧滚动条查看历史";
            ScrollToBottom();
        }

        public void Play(BattleResult result, FighterState playerState, FighterState enemyState, int roundNumber,
            Action<bool> onComplete)
        {
            if (playback != null) StopCoroutine(playback);
            player.ResetForBattle(playerState.HeroId, playerState.Name, playerState.MaxHealth);
            enemy.ResetForBattle(enemyState.HeroId, enemyState.Name, enemyState.MaxHealth);
            PushLog(logLines.Count == 0
                ? $"━━━━ 第 {roundNumber} 轮战斗 ━━━━"
                : $"\n━━━━ 第 {roundNumber} 轮战斗 ━━━━");
            playback = StartCoroutine(PlaybackRoutine(result, onComplete));
        }

        private IEnumerator PlaybackRoutine(BattleResult result, Action<bool> onComplete)
        {
            int previousTick = 0;
            foreach (BattleViewEvent item in result.Events)
            {
                int delta = Mathf.Max(0, item.Tick - previousTick);
                float wait = item.Tick == previousTick ? .055f : Mathf.Min(.42f, delta / 1000f / 3.15f);
                if (wait > 0) yield return new WaitForSecondsRealtime(wait);
                previousTick = item.Tick;
                clockText.text = $"{item.Tick / 1000f:00.0}s";
                Present(item);
            }

            yield return new WaitForSecondsRealtime(.9f);
            playback = null;
            onComplete?.Invoke(result.PlayerWon);
        }

        private void Present(BattleViewEvent item)
        {
            FighterView source = View(item.SourceUnitId);
            FighterView target = View(item.TargetUnitId);
            switch (item.Type)
            {
                case BattleViewEventType.BattleStarted:
                    PushLog("▶ 斗技协议启动");
                    break;

                case BattleViewEventType.AttackStarted:
                    source?.Attack();
                    break;

                case BattleViewEventType.AbilityStarted:
                    AbilityPresentationConfig config = PresentationCatalog.Get(item.AbilityId);
                    Color castColor = UiFactory.Hex(config.accent);
                    source?.Cast(castColor, config.castLabel);
                    StartCoroutine(ShowBanner(item.AbilityName, castColor));
                    PushLog($"◆ {Name(item.SourceUnitId)} 触发 {item.AbilityName}");
                    break;

                case BattleViewEventType.DodgeSucceeded:
                    target?.Dodge();
                    SpawnFloating(target, "闪避", UiFactory.Lime, 32);
                    PushLog($"↗ {Name(item.TargetUnitId)} 闪避成功");
                    break;

                case BattleViewEventType.DamageResolved:
                    Color damageColor = ElementColor(item.Element);
                    target?.Hit(damageColor, item.IsCritical);
                    target?.SetHealth(item.HealthAfter, item.MaxHealth);
                    if (item.Value > 0)
                    {
                        string value = item.IsCritical ? $"暴击 -{item.Value}" : $"-{item.Value}";
                        SpawnFloating(target, value, damageColor, item.IsCritical ? 39 : 31);
                        PushLog($"{Name(item.TargetUnitId)} 受到 {item.Value} 点{ElementName(item.Element)}伤害");
                    }
                    else if (!string.IsNullOrEmpty(item.Note))
                    {
                        PushLog($"⌛ {item.Note}");
                    }
                    break;

                case BattleViewEventType.HealResolved:
                    source?.Heal();
                    source?.SetHealth(item.HealthAfter, item.MaxHealth);
                    SpawnFloating(source, $"+{item.Value}", UiFactory.Hex("#58FFB5"), 30);
                    PushLog($"♥ {Name(item.SourceUnitId)} 回复 {item.Value} 点生命");
                    break;

                case BattleViewEventType.EnergyChanged:
                    source?.SetEnergy(item.EnergyAfter, item.MaxEnergy);
                    break;

                case BattleViewEventType.BuffAdded:
                    Color buffColor = ElementColor(item.Element);
                    target?.SetBuff(item.Note, buffColor);
                    SpawnFloating(target, item.Note, buffColor, 25);
                    PushLog($"● {Name(item.TargetUnitId)} 获得 {item.Note}");
                    break;

                case BattleViewEventType.BuffRemoved:
                    PushLog($"○ {Name(item.TargetUnitId)} 的灼烧消散");
                    break;

                case BattleViewEventType.UnitDied:
                    target?.Die();
                    PushLog($"✕ {Name(item.TargetUnitId)} 失去战斗能力");
                    break;

                case BattleViewEventType.BattleEnded:
                    StartCoroutine(ShowBanner(item.PlayerWon ? "本轮胜利" : "本轮失利",
                        item.PlayerWon ? UiFactory.Lime : UiFactory.Pink));
                    break;
            }
        }

        private FighterView View(int id)
        {
            if (id == 1) return player;
            if (id == 2) return enemy;
            return null;
        }

        private static string Name(int id)
        {
            return id == 1 ? "我方" : id == 2 ? "敌方" : "单位";
        }

        private void SpawnFloating(FighterView target, string value, Color color, int size)
        {
            if (target == null) return;
            Text text = UiFactory.Text("Floating Text", overlayLayer, value, size, color,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            text.raycastTarget = false;
            UiFactory.AddOutline(text, new Color(0, 0, 0, .85f), new Vector2(2, -2));
            RectTransform rect = text.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(320, 72);
            rect.anchoredPosition = target.Root.anchoredPosition + new Vector2(UnityEngine.Random.Range(-25, 25), 100);
            StartCoroutine(FloatAndFade(text));
        }

        private IEnumerator FloatAndFade(Text text)
        {
            RectTransform rect = text.rectTransform;
            Vector2 start = rect.anchoredPosition;
            Color startColor = text.color;
            float elapsed = 0;
            while (elapsed < .78f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / .78f);
                rect.anchoredPosition = start + Vector2.up * Mathf.Lerp(0, 82, t);
                text.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
                yield return null;
            }
            Destroy(text.gameObject);
        }

        private IEnumerator ShowBanner(string value, Color color)
        {
            abilityBanner.text = value;
            abilityBanner.color = color;
            abilityBanner.rectTransform.localScale = Vector3.one * .8f;
            float elapsed = 0;
            while (elapsed < .12f)
            {
                elapsed += Time.unscaledDeltaTime;
                abilityBanner.rectTransform.localScale = Vector3.Lerp(Vector3.one * .8f, Vector3.one,
                    elapsed / .12f);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(.4f);
            if (abilityBanner.text == value) abilityBanner.text = string.Empty;
        }

        private void PushLog(string value)
        {
            bool contentFits = logScroll != null && logScroll.viewport != null &&
                               logText.rectTransform.rect.height <= logScroll.viewport.rect.height + 1f;
            bool followBottom = logScroll == null || logLines.Count == 0 || contentFits ||
                                logScroll.verticalNormalizedPosition <= .08f;
            logLines.Enqueue(value);
            while (logLines.Count > 800) logLines.Dequeue();
            logText.text = string.Join("\n", logLines.ToArray());
            if (followBottom) ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (logScroll == null) return;
            if (scrollRoutine != null) StopCoroutine(scrollRoutine);
            scrollRoutine = StartCoroutine(ScrollToBottomRoutine());
        }

        private IEnumerator ScrollToBottomRoutine()
        {
            yield return null;
            Canvas.ForceUpdateCanvases();
            logScroll.verticalNormalizedPosition = 0f;
            scrollRoutine = null;
        }

        private static Color ElementColor(string element)
        {
            switch (element)
            {
                case "Lightning": return UiFactory.Cyan;
                case "Fire": return UiFactory.Hex("#FF8A3D");
                case "Frost": return UiFactory.Hex("#7FE7FF");
                case "Critical": return UiFactory.Hex("#FFD166");
                case "Arcane": return UiFactory.Hex("#B996FF");
                case "Dodge": return UiFactory.Lime;
                case "Heal": return UiFactory.Hex("#58FFB5");
                case "Void": return UiFactory.Hex("#B996FF");
                default: return UiFactory.White;
            }
        }

        private static string ElementName(string element)
        {
            switch (element)
            {
                case "Lightning": return "雷电";
                case "Fire": return "火焰";
                case "Frost": return "冰霜";
                case "Critical": return "暴击";
                case "Arcane": return "奥术";
                case "Void": return "裁定";
                default: return "物理";
            }
        }
    }
}
