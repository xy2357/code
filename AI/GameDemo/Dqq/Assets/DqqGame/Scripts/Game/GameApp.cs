using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DqqGame.Combat;
using DqqGame.Network;
using DqqGame.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace DqqGame
{
    public sealed class GameApp : MonoBehaviour
    {
        private Canvas canvas;
        private RectTransform arena;
        private RectTransform overlayLayer;
        private RectTransform welcomeOverlay;
        private RectTransform heroOverlay;
        private RectTransform heroCards;
        private RectTransform draftOverlay;
        private RectTransform draftCards;
        private RectTransform resultOverlay;
        private RectTransform matchingOverlay;
        private Text draftTitle;
        private Text resultTitle;
        private Text resultSubtitle;
        private Text roundText;
        private Text recordText;
        private Text buildText;
        private Text battleLog;
        private ScrollRect battleLogScroll;
        private Text clockText;
        private Text matchingStatus;
        private Text rosterText;
        private FighterView playerView;
        private FighterView enemyView;
        private BattlePresenter presenter;
        private MatchClient matchClient;
        private MatchSession onlineSession;

        private BuildState build = new BuildState();
        private int round = 1;
        private int wins;
        private int losses;
        private int runSeed = 20260803;
        private bool battleRunning;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Boot()
        {
            if (FindFirstObjectByType<GameApp>() != null) return;
            new GameObject("DQQ Game App", typeof(GameApp));
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            GameConfig.EnsureLoaded();
            BuildInterface();
            ShowWelcome();

            string[] args = Environment.GetCommandLineArgs();
            if (Array.IndexOf(args, "-autoplaycapture") >= 0)
                StartCoroutine(AutomatedCapture());
        }

        private void BuildInterface()
        {
            canvas = UiFactory.CreateCanvas();
            RectTransform root = canvas.GetComponent<RectTransform>();

            UiFactory.Rect("Background", root, UiFactory.Background, Vector2.zero, Vector2.one,
                Vector2.zero, Vector2.zero, UiFactory.RoundedSprite);
            CreateBackdrop(root);

            RectTransform header = UiFactory.Rect("Header", root, new Color(.055f, .075f, .14f, .96f),
                new Vector2(0, .885f), new Vector2(1, 1), new Vector2(40, -18), new Vector2(-40, -18));
            Text logo = UiFactory.Text("Logo", header, "DQQ // 蛐蛐协议", 40, UiFactory.White,
                TextAnchor.MiddleLeft, FontStyle.Bold);
            logo.rectTransform.offsetMin = new Vector2(34, 0);
            logo.rectTransform.offsetMax = new Vector2(-850, 0);
            Text tag = UiFactory.Text("Tag", header, "AUTO-BATTLER · BUILD LAB", 18, UiFactory.Cyan,
                TextAnchor.MiddleLeft, FontStyle.Bold);
            tag.rectTransform.offsetMin = new Vector2(500, 0);
            tag.rectTransform.offsetMax = new Vector2(-800, 0);

            roundText = UiFactory.Text("Round", header, "ROUND 01", 24, UiFactory.White,
                TextAnchor.MiddleRight, FontStyle.Bold);
            roundText.rectTransform.offsetMin = new Vector2(1200, 0);
            roundText.rectTransform.offsetMax = new Vector2(-340, 0);
            recordText = UiFactory.Text("Record", header, "胜 0/5   ·   败 0/10", 20, UiFactory.Muted,
                TextAnchor.MiddleRight, FontStyle.Bold);
            recordText.rectTransform.offsetMin = new Vector2(1430, 0);
            recordText.rectTransform.offsetMax = new Vector2(-34, 0);

            arena = UiFactory.Rect("Arena", root, new Color(.045f, .06f, .115f, .96f),
                new Vector2(.02f, .285f), new Vector2(.75f, .865f), Vector2.zero, Vector2.zero);
            CreateArenaLines(arena);

            RectTransform arenaHeader = UiFactory.Rect("Arena Header", arena, new Color(.08f, .11f, .20f, .92f),
                new Vector2(0, .87f), new Vector2(1, 1), new Vector2(16, -12), new Vector2(-16, -10));
            UiFactory.Text("Arena Label", arenaHeader, "NEON PIT // 自动演算擂台", 21, UiFactory.Muted,
                TextAnchor.MiddleLeft, FontStyle.Bold).rectTransform.offsetMin = new Vector2(24, 0);
            clockText = UiFactory.Text("Clock", arenaHeader, "00.0s", 24, UiFactory.Cyan,
                TextAnchor.MiddleRight, FontStyle.Bold);
            clockText.rectTransform.offsetMax = new Vector2(-24, 0);

            GameObject playerController = new GameObject("Player View Controller", typeof(RectTransform), typeof(FighterView));
            playerController.transform.SetParent(arena, false);
            playerView = playerController.GetComponent<FighterView>();
            playerView.Build(arena, true);

            GameObject enemyController = new GameObject("Enemy View Controller", typeof(RectTransform), typeof(FighterView));
            enemyController.transform.SetParent(arena, false);
            enemyView = enemyController.GetComponent<FighterView>();
            enemyView.Build(arena, false);

            Text vs = UiFactory.Text("Versus", arena, "VS", 52, new Color(1, 1, 1, .16f),
                TextAnchor.MiddleCenter, FontStyle.Bold);
            vs.raycastTarget = false;

            Text banner = UiFactory.Text("Ability Banner", arena, string.Empty, 36, UiFactory.Cyan,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            banner.rectTransform.anchorMin = new Vector2(.2f, .69f);
            banner.rectTransform.anchorMax = new Vector2(.8f, .82f);
            UiFactory.AddOutline(banner, new Color(0, 0, 0, .9f), new Vector2(3, -3));

            overlayLayer = UiFactory.Rect("Arena Overlay Layer", arena, Color.clear,
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            overlayLayer.GetComponent<Image>().raycastTarget = false;

            RectTransform side = UiFactory.Rect("Combat Feed", root, new Color(.055f, .075f, .14f, .96f),
                new Vector2(.765f, .285f), new Vector2(.98f, .865f), Vector2.zero, Vector2.zero);
            Text feedTitle = UiFactory.Text("Feed Title", side, "战斗事件流", 25, UiFactory.White,
                TextAnchor.UpperLeft, FontStyle.Bold);
            feedTitle.rectTransform.offsetMin = new Vector2(28, 0);
            feedTitle.rectTransform.offsetMax = new Vector2(-20, -28);
            RectTransform logArea = UiFactory.Rect("Battle Log Scroll", side, new Color(.025f, .035f, .075f, .5f),
                new Vector2(0, .29f), new Vector2(1, .81f), new Vector2(22, 0), new Vector2(-18, 0),
                UiFactory.RoundedSprite);
            battleLogScroll = logArea.gameObject.AddComponent<ScrollRect>();
            battleLogScroll.horizontal = false;
            battleLogScroll.vertical = true;
            battleLogScroll.movementType = ScrollRect.MovementType.Clamped;
            battleLogScroll.scrollSensitivity = 34f;

            RectTransform logViewport = UiFactory.Rect("Viewport", logArea, new Color(0, 0, 0, .001f),
                Vector2.zero, Vector2.one, new Vector2(10, 8), new Vector2(-20, -8), UiFactory.RoundedSprite);
            logViewport.gameObject.AddComponent<RectMask2D>();
            battleLog = UiFactory.Text("Battle Log", logViewport,
                "逻辑层待机\n等待构筑完成后开始演算", 18, UiFactory.Muted, TextAnchor.UpperLeft);
            battleLog.lineSpacing = 1.28f;
            RectTransform logContent = battleLog.rectTransform;
            logContent.anchorMin = new Vector2(0, 1);
            logContent.anchorMax = new Vector2(1, 1);
            logContent.pivot = new Vector2(.5f, 1);
            logContent.anchoredPosition = Vector2.zero;
            logContent.sizeDelta = new Vector2(-4, 0);
            ContentSizeFitter logFitter = battleLog.gameObject.AddComponent<ContentSizeFitter>();
            logFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            logFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            RectTransform scrollTrack = UiFactory.Rect("Scrollbar", logArea, new Color(.12f, .16f, .26f, .9f),
                new Vector2(1, 0), new Vector2(1, 1), new Vector2(-11, 3), new Vector2(-3, -3),
                UiFactory.RoundedSprite);
            Scrollbar scrollbar = scrollTrack.gameObject.AddComponent<Scrollbar>();
            RectTransform slidingArea = UiFactory.Rect("Sliding Area", scrollTrack, Color.clear,
                Vector2.zero, Vector2.one, new Vector2(1, 2), new Vector2(-1, -2), UiFactory.RoundedSprite);
            RectTransform handle = UiFactory.Rect("Handle", slidingArea, new Color(UiFactory.Cyan.r, UiFactory.Cyan.g,
                    UiFactory.Cyan.b, .72f), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero,
                UiFactory.RoundedSprite);
            scrollbar.handleRect = handle;
            scrollbar.targetGraphic = handle.GetComponent<Image>();
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            battleLogScroll.viewport = logViewport;
            battleLogScroll.content = logContent;
            battleLogScroll.verticalScrollbar = scrollbar;
            battleLogScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;

            RectTransform ruleCard = UiFactory.Rect("Rule Card", side, UiFactory.PanelLight,
                new Vector2(0, 0), new Vector2(1, .27f), new Vector2(18, 18), new Vector2(-18, -8));
            rosterText = UiFactory.Text("Rule", ruleCard,
                "本局协议\n\n· 自动攻击与释放技能\n· 初始10点生命，失败扣1点\n· 每轮之间三选一强化", 18, UiFactory.Muted,
                TextAnchor.UpperLeft);
            rosterText.rectTransform.offsetMin = new Vector2(22, 16);
            rosterText.rectTransform.offsetMax = new Vector2(-16, -18);

            RectTransform buildBar = UiFactory.Rect("Build Bar", root, new Color(.055f, .075f, .14f, .98f),
                new Vector2(.02f, .035f), new Vector2(.98f, .255f), Vector2.zero, Vector2.zero);
            Text buildTitle = UiFactory.Text("Build Title", buildBar, "当前构筑", 24, UiFactory.White,
                TextAnchor.UpperLeft, FontStyle.Bold);
            buildTitle.rectTransform.offsetMin = new Vector2(28, 0);
            buildTitle.rectTransform.offsetMax = new Vector2(-1500, -24);
            buildText = UiFactory.Text("Build", buildBar, "尚未选择强化", 18, UiFactory.Muted,
                TextAnchor.UpperLeft);
            buildText.lineSpacing = 1.15f;
            buildText.rectTransform.offsetMin = new Vector2(28, 14);
            buildText.rectTransform.offsetMax = new Vector2(-28, -58);

            presenter = gameObject.AddComponent<BattlePresenter>();
            matchClient = gameObject.AddComponent<MatchClient>();
            presenter.Initialize(playerView, enemyView, overlayLayer, banner, clockText, battleLog, battleLogScroll);
            playerView.ResetForBattle(1, "齿轮斗士", 920);
            enemyView.ResetForBattle(2, "猩红猎手", 760);

            BuildWelcomeOverlay(root);
            BuildHeroOverlay(root);
            BuildMatchingOverlay(root);
            BuildDraftOverlay(root);
            BuildResultOverlay(root);
        }

        private void CreateBackdrop(RectTransform root)
        {
            RectTransform glowA = UiFactory.Rect("Cyan Glow", root, new Color(.1f, .75f, 1f, .12f),
                new Vector2(0, 0), new Vector2(0, 0), Vector2.zero, Vector2.zero, UiFactory.SoftCircleSprite);
            glowA.sizeDelta = new Vector2(1100, 1100);
            glowA.anchoredPosition = new Vector2(150, 80);
            RectTransform glowB = UiFactory.Rect("Pink Glow", root, new Color(1f, .15f, .55f, .1f),
                new Vector2(1, 1), new Vector2(1, 1), Vector2.zero, Vector2.zero, UiFactory.SoftCircleSprite);
            glowB.sizeDelta = new Vector2(900, 900);
            glowB.anchoredPosition = new Vector2(-120, -80);
        }

        private static void CreateArenaLines(RectTransform parent)
        {
            for (int i = 0; i < 7; i++)
            {
                RectTransform line = UiFactory.Rect("Grid H", parent, new Color(.25f, .8f, 1f, .055f),
                    new Vector2(.05f, .16f + i * .09f), new Vector2(.95f, .16f + i * .09f),
                    Vector2.zero, new Vector2(0, 2));
                line.GetComponent<Image>().sprite = null;
            }
        }

        private void BuildWelcomeOverlay(RectTransform root)
        {
            welcomeOverlay = UiFactory.Rect("Welcome Overlay", root, new Color(.025f, .035f, .075f, .97f),
                new Vector2(.18f, .16f), new Vector2(.82f, .86f), Vector2.zero, Vector2.zero);
            Text eyebrow = UiFactory.Text("Eyebrow", welcomeOverlay, "PROJECT DQQ / BUILD 01", 20,
                UiFactory.Cyan, TextAnchor.UpperCenter, FontStyle.Bold);
            eyebrow.rectTransform.offsetMax = new Vector2(0, -56);
            Text title = UiFactory.Text("Title", welcomeOverlay, "电子斗蛐蛐", 72, UiFactory.White,
                TextAnchor.UpperCenter, FontStyle.Bold);
            title.rectTransform.offsetMax = new Vector2(0, -115);
            Text subtitle = UiFactory.Text("Subtitle", welcomeOverlay,
                "选出技能与改造，养出属于你的怪物\n战斗全自动——胜负在开打之前已经写进构筑", 27,
                UiFactory.Muted, TextAnchor.MiddleCenter);
            subtitle.lineSpacing = 1.25f;
            subtitle.rectTransform.offsetMin = new Vector2(110, 170);
            subtitle.rectTransform.offsetMax = new Vector2(-110, -260);
            RectTransform buttonSlot = UiFactory.Rect("Start Slot", welcomeOverlay, Color.clear,
                new Vector2(.31f, .08f), new Vector2(.69f, .21f), Vector2.zero, Vector2.zero);
            UiFactory.Button("Start", buttonSlot, "选择英雄并开始  →", UiFactory.Lime, BeginRun);
            Text footer = UiFactory.Text("Footer", welcomeOverlay,
                "事件驱动技能 · 确定性战斗日志 · 表现与逻辑分离", 17,
                new Color(UiFactory.Muted.r, UiFactory.Muted.g, UiFactory.Muted.b, .65f),
                TextAnchor.LowerCenter);
            footer.rectTransform.offsetMin = new Vector2(0, 22);
        }

        private void BuildHeroOverlay(RectTransform root)
        {
            heroOverlay = UiFactory.Rect("Hero Overlay", root, new Color(.025f, .035f, .075f, .99f),
                new Vector2(.07f, .07f), new Vector2(.93f, .93f), Vector2.zero, Vector2.zero);
            Text title = UiFactory.Text("Hero Title", heroOverlay, "选择你的初始英雄", 42, UiFactory.White,
                TextAnchor.UpperCenter, FontStyle.Bold);
            title.rectTransform.offsetMax = new Vector2(0, -28);
            Text hint = UiFactory.Text("Hero Hint", heroOverlay,
                "英雄提供一个固有被动和一个100能量大招，但不会限制后续流派", 19,
                UiFactory.Muted, TextAnchor.UpperCenter);
            hint.rectTransform.offsetMax = new Vector2(0, -84);
            heroCards = UiFactory.Rect("Hero Cards", heroOverlay, Color.clear,
                new Vector2(.035f, .055f), new Vector2(.965f, .82f), Vector2.zero, Vector2.zero);
            heroCards.GetComponent<Image>().raycastTarget = false;

            foreach (HeroConfig hero in GameConfig.Heroes)
                CreateHeroCard(hero);
            heroOverlay.gameObject.SetActive(false);
        }

        private void CreateHeroCard(HeroConfig hero)
        {
            int index = hero.heroId - 1;
            int column = index % 3;
            int row = index / 3;
            const float gapX = .018f;
            const float gapY = .045f;
            float width = (1f - gapX * 4f) / 3f;
            float height = (1f - gapY * 3f) / 2f;
            float minX = gapX + column * (width + gapX);
            float maxY = 1f - gapY - row * (height + gapY);
            RectTransform card = UiFactory.Rect($"Hero {hero.heroId}", heroCards, UiFactory.PanelLight,
                new Vector2(minX, maxY - height), new Vector2(minX + width, maxY), Vector2.zero, Vector2.zero);
            Color accent = UiFactory.Hex(hero.accent);
            UiFactory.Rect("Accent", card, accent, new Vector2(0, .96f), Vector2.one, Vector2.zero, Vector2.zero)
                .GetComponent<Image>().sprite = null;
            RectTransform portrait = UiFactory.Rect("3D Portrait", card,
                new Color(accent.r, accent.g, accent.b, .18f), new Vector2(.035f, .25f),
                new Vector2(.36f, .91f), Vector2.zero, Vector2.zero, UiFactory.AdventureHexSprite);
            GameObject previewObject = new GameObject("Hero Preview Controller", typeof(RectTransform), typeof(HeroRenderRig));
            previewObject.transform.SetParent(portrait, false);
            HeroRenderRig preview = previewObject.GetComponent<HeroRenderRig>();
            preview.Build(portrait, true, 256);
            preview.SetHero(hero);
            Text school = UiFactory.Text("School", card, SchoolName(hero.school), 16, accent,
                TextAnchor.UpperLeft, FontStyle.Bold);
            school.rectTransform.anchorMin = new Vector2(.39f, 0);
            school.rectTransform.offsetMin = new Vector2(0, 0);
            school.rectTransform.offsetMax = new Vector2(-20, -18);
            Text name = UiFactory.Text("Name", card, hero.heroName, 28, UiFactory.White,
                TextAnchor.UpperLeft, FontStyle.Bold);
            name.rectTransform.anchorMin = new Vector2(.39f, 0);
            name.rectTransform.offsetMin = new Vector2(0, 0);
            name.rectTransform.offsetMax = new Vector2(-20, -52);
            Text detail = UiFactory.Text("Detail", card,
                $"{hero.title}\n被动 · {hero.passiveName}：{hero.passiveDescription}\n大招 · {hero.ultimateName}：{hero.ultimateDescription}",
                14, UiFactory.Muted, TextAnchor.UpperLeft);
            detail.lineSpacing = 1.0f;
            detail.rectTransform.anchorMin = new Vector2(.39f, 0);
            detail.rectTransform.offsetMin = new Vector2(0, 52);
            detail.rectTransform.offsetMax = new Vector2(-20, -98);
            RectTransform buttonSlot = UiFactory.Rect("Select Slot", card, Color.clear,
                new Vector2(.16f, .055f), new Vector2(.84f, .22f), Vector2.zero, Vector2.zero);
            UiFactory.Button("Select", buttonSlot, "选择", accent, () => ChooseHero(hero));
        }

        private void BuildDraftOverlay(RectTransform root)
        {
            draftOverlay = UiFactory.Rect("Draft Overlay", root, new Color(.025f, .035f, .075f, .985f),
                new Vector2(.10f, .12f), new Vector2(.90f, .89f), Vector2.zero, Vector2.zero);
            draftTitle = UiFactory.Text("Draft Title", draftOverlay, "选择一项改造", 42, UiFactory.White,
                TextAnchor.UpperCenter, FontStyle.Bold);
            draftTitle.rectTransform.offsetMax = new Vector2(0, -42);
            Text hint = UiFactory.Text("Draft Hint", draftOverlay, "每次选择都会永久影响本轮构筑", 20,
                UiFactory.Muted, TextAnchor.UpperCenter);
            hint.rectTransform.offsetMax = new Vector2(0, -105);
            draftCards = UiFactory.Rect("Cards", draftOverlay, Color.clear,
                new Vector2(.035f, .12f), new Vector2(.965f, .77f), Vector2.zero, Vector2.zero);
            draftCards.GetComponent<Image>().raycastTarget = false;
            draftOverlay.gameObject.SetActive(false);
        }

        private void BuildMatchingOverlay(RectTransform root)
        {
            matchingOverlay = UiFactory.Rect("Matching Overlay", root, new Color(.025f, .035f, .075f, .99f),
                new Vector2(.28f, .25f), new Vector2(.72f, .75f), Vector2.zero, Vector2.zero);
            Text title = UiFactory.Text("Matching Title", matchingOverlay, "四人匹配", 48, UiFactory.White,
                TextAnchor.UpperCenter, FontStyle.Bold);
            title.rectTransform.offsetMax = new Vector2(0, -68);
            Text mode = UiFactory.Text("Mode", matchingOverlay,
                "4 PLAYERS · 10 LIVES · LAST ONE STANDING", 18, UiFactory.Cyan,
                TextAnchor.UpperCenter, FontStyle.Bold);
            mode.rectTransform.offsetMax = new Vector2(0, -132);
            matchingStatus = UiFactory.Text("Status", matchingOverlay, "正在连接匹配服务器…", 25,
                UiFactory.Muted, TextAnchor.MiddleCenter);
            matchingStatus.rectTransform.offsetMin = new Vector2(30, 60);
            matchingStatus.rectTransform.offsetMax = new Vector2(-30, -180);
            Text hint = UiFactory.Text("Hint", matchingOverlay,
                "等待超过1.5秒将自动补入机器人", 17, UiFactory.Muted, TextAnchor.LowerCenter);
            hint.rectTransform.offsetMin = new Vector2(0, 30);
            matchingOverlay.gameObject.SetActive(false);
        }

        private void BuildResultOverlay(RectTransform root)
        {
            resultOverlay = UiFactory.Rect("Result Overlay", root, new Color(.025f, .035f, .075f, .985f),
                new Vector2(.24f, .20f), new Vector2(.76f, .80f), Vector2.zero, Vector2.zero);
            resultTitle = UiFactory.Text("Result Title", resultOverlay, "构筑完成", 58, UiFactory.Lime,
                TextAnchor.UpperCenter, FontStyle.Bold);
            resultTitle.rectTransform.offsetMax = new Vector2(0, -70);
            resultSubtitle = UiFactory.Text("Result Subtitle", resultOverlay, string.Empty, 25,
                UiFactory.Muted, TextAnchor.MiddleCenter);
            resultSubtitle.rectTransform.offsetMin = new Vector2(60, 120);
            resultSubtitle.rectTransform.offsetMax = new Vector2(-60, -170);
            RectTransform buttonSlot = UiFactory.Rect("Restart Slot", resultOverlay, Color.clear,
                new Vector2(.26f, .08f), new Vector2(.74f, .22f), Vector2.zero, Vector2.zero);
            UiFactory.Button("Restart", buttonSlot, "再养一只  →", UiFactory.Lime, BeginRun);
            resultOverlay.gameObject.SetActive(false);
        }

        private void ShowWelcome()
        {
            welcomeOverlay.gameObject.SetActive(true);
            heroOverlay.gameObject.SetActive(false);
            matchingOverlay.gameObject.SetActive(false);
            draftOverlay.gameObject.SetActive(false);
            resultOverlay.gameObject.SetActive(false);
        }

        private void BeginRun()
        {
            if (battleRunning) return;
            build = new BuildState();
            round = 1;
            wins = 0;
            losses = 0;
            runSeed += 97;
            welcomeOverlay.gameObject.SetActive(false);
            resultOverlay.gameObject.SetActive(false);
            heroOverlay.gameObject.SetActive(true);
            onlineSession = null;
            presenter.ResetHistory();
            rosterText.text = "本局协议\n\n· 四人匹配，失败扣1生命\n· 三选一强化后自动战斗\n· 最后一名存活者获胜";
            UpdateHeader();
            UpdateBuildDisplay();
        }

        private void ChooseHero(HeroConfig hero)
        {
            build.HeroId = hero.heroId;
            heroOverlay.gameObject.SetActive(false);
            UpdateBuildDisplay();
            StartCoroutine(BeginMatchmaking(hero));
        }

        private IEnumerator BeginMatchmaking(HeroConfig hero)
        {
            matchingOverlay.gameObject.SetActive(true);
            matchingStatus.text = "正在连接匹配服务器…";
            bool finished = false;
            string failure = null;
            yield return matchClient.Join(hero.heroId,
                status => matchingStatus.text = status,
                session => { onlineSession = session; finished = true; },
                error => { failure = error; finished = true; });
            while (!finished) yield return null;
            matchingOverlay.gameObject.SetActive(false);
            if (onlineSession != null)
            {
                round = onlineSession.Match.round;
                UpdateOnlineRecord();
                ShowDraft($"四人房间 · 第{round}轮改造");
            }
            else
            {
                battleLog.text = failure ?? "服务器不可用，训练模式启动";
                ShowDraft($"{hero.heroName} · 训练模式");
            }
        }

        private void ShowDraft(string title)
        {
            battleRunning = false;
            draftTitle.text = title;
            draftOverlay.gameObject.SetActive(true);
            for (int i = draftCards.childCount - 1; i >= 0; i--)
                Destroy(draftCards.GetChild(i).gameObject);

            List<UpgradeConfig> options = PickDraftOptions();
            for (int i = 0; i < options.Count; i++)
                CreateDraftCard(options[i], i);
        }

        private List<UpgradeConfig> PickDraftOptions()
        {
            List<UpgradeConfig> pool = new List<UpgradeConfig>();
            foreach (UpgradeConfig config in GameConfig.Upgrades)
            {
                if (config.unique && config.addAbilityId != 0 && build.HasAbility(config.addAbilityId)) continue;
                pool.Add(config);
            }

            System.Random rng = new System.Random(runSeed + round * 193 + wins * 31 + losses * 17);
            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                UpgradeConfig temp = pool[i];
                pool[i] = pool[j];
                pool[j] = temp;
            }
            List<UpgradeConfig> result = new List<UpgradeConfig>();
            string preferredSchool = GameConfig.Hero(build.HeroId).school;
            UpgradeConfig preferred = pool.Find(item => item.school == preferredSchool);
            if (preferred != null)
            {
                result.Add(preferred);
                pool.Remove(preferred);
            }
            while (result.Count < 3 && pool.Count > 0)
            {
                result.Add(pool[0]);
                pool.RemoveAt(0);
            }
            return result;
        }

        private void CreateDraftCard(UpgradeConfig config, int index)
        {
            float gap = .025f;
            float width = (1f - gap * 4f) / 3f;
            float minX = gap + index * (width + gap);
            RectTransform slot = UiFactory.Rect($"Card {config.upgradeId}", draftCards, UiFactory.PanelLight,
                new Vector2(minX, 0), new Vector2(minX + width, 1), Vector2.zero, Vector2.zero);
            Color accent = UiFactory.Hex(config.accent);
            RectTransform line = UiFactory.Rect("Accent", slot, accent,
                new Vector2(0, .965f), new Vector2(1, 1), Vector2.zero, Vector2.zero);
            line.GetComponent<Image>().sprite = null;

            RectTransform icon = UiFactory.Rect("Icon", slot, new Color(accent.r, accent.g, accent.b, .18f),
                new Vector2(.5f, .67f), new Vector2(.5f, .67f), Vector2.zero, Vector2.zero, UiFactory.AdventureHexSprite);
            icon.sizeDelta = new Vector2(118, 118);
            Text iconText = UiFactory.Text("Icon Text", icon, config.icon, 43, accent,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            iconText.raycastTarget = false;

            Text name = UiFactory.Text("Name", slot, config.upgradeName, 30, UiFactory.White,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            name.rectTransform.anchorMin = new Vector2(.05f, .43f);
            name.rectTransform.anchorMax = new Vector2(.95f, .59f);
            Text description = UiFactory.Text("Description", slot, config.description, 21, UiFactory.Muted,
                TextAnchor.UpperCenter);
            description.rectTransform.anchorMin = new Vector2(.08f, .25f);
            description.rectTransform.anchorMax = new Vector2(.92f, .43f);
            Text school = UiFactory.Text("School", slot,
                $"{SchoolName(config.school)} · {RarityName(config.rarity)}", 16, accent,
                TextAnchor.UpperCenter, FontStyle.Bold);
            school.rectTransform.anchorMin = new Vector2(.08f, .20f);
            school.rectTransform.anchorMax = new Vector2(.92f, .28f);

            RectTransform buttonSlot = UiFactory.Rect("Button Slot", slot, Color.clear,
                new Vector2(.17f, .07f), new Vector2(.83f, .19f), Vector2.zero, Vector2.zero);
            UiFactory.Button("Choose", buttonSlot, "装载改造", accent, () => ChooseUpgrade(config));
        }

        private void ChooseUpgrade(UpgradeConfig config)
        {
            if (battleRunning) return;
            battleRunning = true;
            build.Apply(config);
            UpdateBuildDisplay();
            draftOverlay.gameObject.SetActive(false);
            if (onlineSession != null)
                StartCoroutine(SubmitUpgradeThenBattle(config));
            else
                StartCoroutine(BeginBattleAfterDelay());
        }

        private IEnumerator SubmitUpgradeThenBattle(UpgradeConfig config)
        {
            bool completed = false;
            bool failed = false;
            yield return matchClient.SubmitUpgrade(config.upgradeId,
                match => { onlineSession.Match = match; completed = true; },
                error => { battleLog.text = error; failed = true; completed = true; });
            while (!completed) yield return null;
            if (failed) onlineSession = null;
            int draftWaits = 0;
            while (onlineSession != null && onlineSession.Match.status == "draft" && draftWaits++ < 40)
            {
                matchingOverlay.gameObject.SetActive(true);
                matchingStatus.text = "等待其他玩家完成构筑…";
                yield return new WaitForSecondsRealtime(.5f);
                bool refreshed = false;
                yield return matchClient.Refresh(match => { onlineSession.Match = match; refreshed = true; },
                    error => { battleLog.text = error; refreshed = true; });
                while (!refreshed) yield return null;
            }
            if (onlineSession != null && onlineSession.Match.status == "draft")
            {
                battleLog.text = "等待其他玩家超时，切换训练模式";
                onlineSession = null;
            }
            matchingOverlay.gameObject.SetActive(false);
            yield return BeginBattleAfterDelay();
        }

        private IEnumerator BeginBattleAfterDelay()
        {
            yield return new WaitForSecondsRealtime(.2f);
            roundText.text = $"ROUND {round:00}";
            CombatWorld world;
            if (onlineSession != null && onlineSession.Opponent != null)
                world = new CombatWorld(build, onlineSession.BuildOpponentState(), round,
                    onlineSession.Match.seed + round * 1009);
            else
                world = new CombatWorld(build, round, runSeed + round * 1009);
            BattleResult result = world.Run();
            presenter.Play(result, world.Player, world.Enemy, round, OnBattleComplete);
        }

        private void OnBattleComplete(bool won)
        {
            if (onlineSession != null)
            {
                StartCoroutine(ReportOnlineResult(won));
                return;
            }
            if (won) wins++; else losses++;
            round++;
            UpdateHeader();
            if (wins >= 5 || losses >= 10)
            {
                ShowRunResult(wins >= 5);
                return;
            }
            StartCoroutine(ShowNextDraft(won));
        }

        private IEnumerator ReportOnlineResult(bool won)
        {
            int reportedRound = onlineSession.Match.round;
            bool completed = false;
            bool failed = false;
            yield return matchClient.SubmitResult(won, $"{onlineSession.Match.seed:X8}-{reportedRound}-{(won ? 1 : 0)}",
                match => { onlineSession.Match = match; completed = true; },
                error => { battleLog.text = error; failed = true; completed = true; });
            while (!completed) yield return null;
            if (failed)
            {
                onlineSession = null;
                if (won) wins++; else losses++;
                round++;
                UpdateHeader();
                ShowDraft("连接中断 · 继续训练");
                yield break;
            }

            while (onlineSession.Match.status == "battle" && onlineSession.Match.round == reportedRound)
            {
                matchingOverlay.gameObject.SetActive(true);
                matchingStatus.text = "等待其他对局结算…";
                yield return new WaitForSecondsRealtime(.5f);
                bool refreshed = false;
                yield return matchClient.Refresh(match => { onlineSession.Match = match; refreshed = true; },
                    error => { battleLog.text = error; refreshed = true; });
                while (!refreshed) yield return null;
            }
            matchingOverlay.gameObject.SetActive(false);
            PlayerDto local = onlineSession.LocalPlayer;
            wins += won ? 1 : 0;
            losses = local == null ? losses : 10 - local.lives;
            round = onlineSession.Match.round;
            UpdateOnlineRecord();

            if (local == null || local.isEliminated || onlineSession.Match.status == "completed")
            {
                bool champion = local != null && local.placement == 1;
                ShowOnlineResult(champion, local?.placement ?? 4);
                yield break;
            }
            yield return new WaitForSecondsRealtime(.35f);
            ShowDraft($"四人房间 · 第{round}轮改造");
        }

        private void ShowOnlineResult(bool champion, int placement)
        {
            battleRunning = false;
            resultOverlay.gameObject.SetActive(true);
            resultTitle.text = champion ? "四人匹配冠军" : $"本局第 {Mathf.Clamp(placement, 2, 4)} 名";
            resultTitle.color = champion ? UiFactory.Lime : UiFactory.Pink;
            resultSubtitle.text = $"战绩  {wins} 胜 {losses} 败\n\n" +
                                  (champion ? "你的构筑存活到了最后。" : "调整英雄与流派组合，再来一局。");
        }

        private void UpdateOnlineRecord()
        {
            if (onlineSession?.Match?.players == null) return;
            roundText.text = $"ROUND {onlineSession.Match.round:00}";
            PlayerDto local = onlineSession.LocalPlayer;
            recordText.text = $"四人房间   生命 {local?.lives ?? 0}/10   ·   存活 " +
                              Array.FindAll(onlineSession.Match.players, item => !item.isEliminated).Length + "/4";
            List<string> roster = new List<string> { "房间排名" };
            Array.Sort(onlineSession.Match.players, (a, b) => b.lives.CompareTo(a.lives));
            foreach (PlayerDto player in onlineSession.Match.players)
            {
                string marker = player.playerId == onlineSession.PlayerId ? "▶" : player.isBot ? "◆" : "·";
                roster.Add($"{marker} {player.displayName}   ♥{player.lives}   {GameConfig.Hero(player.heroId).heroName}");
            }
            rosterText.text = string.Join("\n", roster);
        }

        private IEnumerator ShowNextDraft(bool won)
        {
            yield return new WaitForSecondsRealtime(.45f);
            ShowDraft(won ? "胜利强化 · 三选一" : "败者补强 · 三选一");
        }

        private void ShowRunResult(bool victory)
        {
            battleRunning = false;
            resultOverlay.gameObject.SetActive(true);
            resultTitle.text = victory ? "蛐蛐养成完成" : "本轮构筑终止";
            resultTitle.color = victory ? UiFactory.Lime : UiFactory.Pink;
            resultSubtitle.text = victory
                ? $"最终战绩  {wins} 胜 {losses} 败\n\n这只蛐蛐已经通过霓虹擂台认证。"
                : $"最终战绩  {wins} 胜 {losses} 败\n\n换一种技能联动，再试一次。";
        }

        private void UpdateHeader()
        {
            roundText.text = $"ROUND {round:00}";
            recordText.text = $"胜 {wins}/5   ·   败 {losses}/10";
        }

        private void UpdateBuildDisplay()
        {
            List<string> skills = new List<string>();
            foreach (int id in build.AbilityIds) skills.Add(GameConfig.Ability(id).abilityName);
            string skillText = skills.Count == 0 ? "无技能模块" : string.Join("  /  ", skills);
            List<string> upgrades = new List<string>();
            int upgradeCount = 0;
            foreach (KeyValuePair<string, int> pair in build.UpgradeRanks)
            {
                UpgradeConfig config = GameConfig.Upgrade(pair.Key);
                string name = config == null ? pair.Key : config.upgradeName;
                upgrades.Add(pair.Value > 1 ? $"{name} ×{pair.Value}" : name);
                upgradeCount += pair.Value;
            }
            string upgradeText = upgrades.Count == 0 ? "尚未选择" : string.Join("  ·  ", upgrades);
            HeroConfig hero = GameConfig.Hero(build.HeroId);
            buildText.text =
                $"英雄：{hero.heroName}  [{SchoolName(hero.school)}]    " +
                $"攻击 ×{build.AttackBP / 10000f:0.00}    生命 ×{build.HealthBP / 10000f:0.00}    " +
                $"攻速 ×{build.AttackSpeedBP / 10000f:0.00}    闪避 {build.DodgeBP / 100f:0}%    " +
                $"暴击 {build.CritBP / 100f:0}%    防御 +{build.DefenseFlat}\n" +
                $"技能模块：{skillText}\n" +
                $"已选强化（{upgradeCount}）：{upgradeText}";
        }

        private IEnumerator AutomatedCapture()
        {
            yield return new WaitForSecondsRealtime(.4f);
            welcomeOverlay.gameObject.SetActive(false);
            heroOverlay.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(.65f);
            string directory = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Screenshots"));
            Directory.CreateDirectory(directory);
            ScreenCapture.CaptureScreenshot(Path.Combine(directory, "hero_selection.png"));
            yield return new WaitForSecondsRealtime(.25f);
            heroOverlay.gameObject.SetActive(false);
            build = new BuildState();
            build.HeroId = 6;
            foreach (UpgradeConfig config in GameConfig.Upgrades)
            {
                if (config.upgradeId == "thunder" || config.upgradeId == "counter" || config.upgradeId == "haste")
                    build.Apply(config);
            }
            round = 3;
            wins = 2;
            UpdateHeader();
            UpdateBuildDisplay();
            draftOverlay.gameObject.SetActive(false);
            battleRunning = true;
            StartCoroutine(BeginBattleAfterDelay());
            yield return new WaitForSecondsRealtime(2.4f);
            ScreenCapture.CaptureScreenshot(Path.Combine(directory, "gameplay.png"));
            yield return new WaitForSecondsRealtime(1f);
            Application.Quit();
        }

        private static string SchoolName(string school)
        {
            switch (school)
            {
                case "Basic": return "普攻流";
                case "Critical": return "暴击流";
                case "Ultimate": return "大招流";
                case "Dodge": return "闪避流";
                case "Frost": return "冰霜流";
                case "Burn": return "燃烧流";
                default: return "通用";
            }
        }

        private static string RarityName(string rarity)
        {
            switch (rarity)
            {
                case "Epic": return "史诗";
                case "Rare": return "稀有";
                default: return "普通";
            }
        }
    }
}
