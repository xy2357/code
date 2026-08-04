using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dice21
{
    [DefaultExecutionOrder(100)]
    public sealed class Dice21Game : MonoBehaviour
    {
        private static readonly Color Ink = Hex("#161427");
        private static readonly Color Cream = Hex("#FFF4DE");
        private static readonly Color Muted = Hex("#AAA4B8");
        private static readonly Color Gold = Hex("#FFC94F");
        private static readonly Color Red = Hex("#F25562");
        private static readonly Color Blue = Hex("#35A8FF");
        private static readonly Color TableDark = Hex("#17342E");
        private static readonly Color TableLight = Hex("#315C48");
        private static readonly Color Panel = Hex("#25213C");

        private UIDocument _document;
        private Font _font;
        private Sprite[] _rollSprites;
        private Sprite[] _resultSprites;
        private Texture2D _yellowButton;
        private Texture2D _redButton;
        private Texture2D _blueButton;

        private readonly DieView[] _playerDice = new DieView[2];
        private readonly DieView[] _robotDice = new DieView[2];
        private readonly List<Label> _historyItems = new List<Label>();

        private Label _playerScoreLabel;
        private Label _robotScoreLabel;
        private Label _messageKicker;
        private Label _messageLabel;
        private Label _statusLabel;
        private Label _tipLabel;
        private VisualElement _playerGlow;
        private VisualElement _robotGlow;
        private VisualElement _historyRow;
        private Label _historyEmptyLabel;
        private Button _rollButton;
        private Button _holdButton;
        private Button _helpButton;
        private Button _soundButton;
        private VisualElement _rulesOverlay;
        private VisualElement _resultOverlay;
        private VisualElement _resultCard;
        private Label _resultBadge;
        private Label _resultKicker;
        private Label _resultTitle;
        private Label _resultDescription;
        private Label _resultPlayerScore;
        private Label _resultRobotScore;

        private int _playerScore;
        private int _robotScore;
        private bool _playerHasRolled;
        private bool _busy;
        private bool _gameOver;
        private bool _soundOn = true;
        private AudioSource _audioSource;
        private AudioClip _rollClip;
        private AudioClip _impactClip;
        private AudioClip _scoreClip;
        private AudioClip _winClip;
        private AudioClip _loseClip;
        private RenderTexture _diceRenderTexture;
        private GameObject _diceRenderRoot;

        private const int DiceRenderLayer = 30;
        private const float DicePixelsPerUnit = 100f;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            if (_document.panelSettings == null)
            {
                PanelSettings runtimePanel = Resources.Load<PanelSettings>("Dice21PanelSettings");
                if (runtimePanel == null)
                {
                    runtimePanel = ScriptableObject.CreateInstance<PanelSettings>();
                    runtimePanel.name = "Dice21 Runtime Panel Settings";
                    runtimePanel.scaleMode = PanelScaleMode.ScaleWithScreenSize;
                    runtimePanel.referenceResolution = new Vector2Int(1080, 1920);
                    runtimePanel.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
                    runtimePanel.match = .5f;
                    runtimePanel.sortingOrder = 0;
                }
                _document.panelSettings = runtimePanel;
            }
        }

        private sealed class DieView
        {
            public VisualElement Root;
            public VisualElement Shadow;
            public VisualElement Body;
            public Image Art;
            public Image[] Ghosts;
            public Color Tint;
            public int Value;
            public GameObject Model3D;
            public GameObject Shadow3D;
            public Renderer ShadowRenderer3D;
            public Vector3 BaseWorldPosition;
        }

        private IEnumerator Start()
        {
            yield return null;

            if (_document.rootVisualElement == null)
            {
                Debug.LogError("[Dice21] UIDocument failed to create a root visual element.");
                yield break;
            }

            LoadResources();
            BuildInterface();
            BuildAudio();
            ResetGame();
            Debug.Log("[Dice21] Runtime interface initialized.");
        }

        private void OnDestroy()
        {
            if (_diceRenderTexture == null) return;
            _diceRenderTexture.Release();
            Destroy(_diceRenderTexture);
            _diceRenderTexture = null;
        }

        private void Update()
        {
            if (_gameOver || _busy) return;
            if (Input.GetKeyDown(KeyCode.Space)) RollClicked();
            if (Input.GetKeyDown(KeyCode.Return) && _playerHasRolled) HoldClicked();
        }

        private void LoadResources()
        {
            _font = Font.CreateDynamicFontFromOSFont(
                new[] { "Microsoft YaHei UI", "Microsoft YaHei", "SimHei", "Arial" }, 42);

            Texture2D rollTexture = Resources.Load<Texture2D>("Art/diceroll");
            Texture2D resultTexture = Resources.Load<Texture2D>("Art/diceresult");
            _rollSprites = SliceSheet(rollTexture, 2, 6);
            _resultSprites = SliceSheet(resultTexture, 2, 6);
            _yellowButton = Resources.Load<Texture2D>("Art/button_yellow");
            _redButton = Resources.Load<Texture2D>("Art/button_red");
            _blueButton = Resources.Load<Texture2D>("Art/button_blue");
        }

        private static Sprite[] SliceSheet(Texture2D texture, int columns, int rows)
        {
            if (texture == null) return Array.Empty<Sprite>();
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            int cellWidth = texture.width / columns;
            int cellHeight = texture.height / rows;
            Sprite[] sprites = new Sprite[columns * rows];
            int index = 0;
            for (int row = 0; row < rows; row++)
            {
                int y = texture.height - ((row + 1) * cellHeight);
                for (int column = 0; column < columns; column++)
                {
                    Rect rect = new Rect(column * cellWidth, y, cellWidth, cellHeight);
                    sprites[index++] = Sprite.Create(texture, rect, new Vector2(.5f, .5f), 32f);
                }
            }
            return sprites;
        }

        private void BuildInterface()
        {
            VisualElement root = _document.rootVisualElement;
            root.Clear();
            root.style.flexGrow = 1;
            root.style.backgroundColor = Ink;

            VisualElement backdrop = AddElement(root, "Backdrop");
            Stretch(backdrop);
            backdrop.pickingMode = PickingMode.Ignore;
            backdrop.style.backgroundColor = Hex("#14172B");
            AddBackdropDecor(backdrop);

            VisualElement page = AddElement(root, "PortraitPage");
            Stretch(page);
            page.style.maxWidth = 1080;
            page.style.alignSelf = Align.Center;

            BuildHeader(page);
            BuildArena(page);
            BuildControls(page);
            BuildRulesOverlay(page);
            BuildResultOverlay(page);
        }

        private void AddBackdropDecor(VisualElement parent)
        {
            for (int i = 0; i < 12; i++)
            {
                VisualElement diamond = AddElement(parent, "Diamond");
                diamond.style.position = Position.Absolute;
                diamond.style.width = 75;
                diamond.style.height = 75;
                diamond.style.left = (i % 4) * 300 - 30;
                diamond.style.top = (i / 4) * 650 + 120;
                diamond.style.rotate = new Rotate(45);
                diamond.style.borderLeftWidth = 3;
                diamond.style.borderRightWidth = 3;
                diamond.style.borderTopWidth = 3;
                diamond.style.borderBottomWidth = 3;
                diamond.style.borderLeftColor = new Color(1, 1, 1, .035f);
                diamond.style.borderRightColor = new Color(1, 1, 1, .035f);
                diamond.style.borderTopColor = new Color(1, 1, 1, .035f);
                diamond.style.borderBottomColor = new Color(1, 1, 1, .035f);
            }

            VisualElement violetGlow = AddElement(parent, "VioletGlow");
            SetRect(violetGlow, 140, 450, 800, 800);
            Round(violetGlow, 400);
            violetGlow.style.backgroundColor = new Color(.36f, .22f, .5f, .08f);
        }

        private void BuildHeader(VisualElement page)
        {
            VisualElement headerShadow = AddElement(page, "HeaderShadow");
            SetRect(headerShadow, 155, 38, 770, 126);
            Round(headerShadow, 34);
            headerShadow.style.backgroundColor = new Color(0, 0, 0, .42f);

            VisualElement headerPlate = AddElement(page, "HeaderPlate");
            SetRect(headerPlate, 150, 24, 780, 126);
            Round(headerPlate, 34);
            headerPlate.style.backgroundColor = Hex("#3A304D");
            SetBorder(headerPlate, 6, Ink);

            VisualElement headerHighlight = AddElement(headerPlate, "HeaderHighlight");
            SetRect(headerHighlight, 24, 12, 732, 4);
            Round(headerHighlight, 3);
            headerHighlight.style.backgroundColor = new Color(1, 1, 1, .14f);

            Label title = AddLabel(page, "二十一点", 68, Cream, FontStyle.Bold);
            SetRect(title, 220, 43, 640, 86);
            title.style.textShadow = new TextShadow { offset = new Vector2(0, 5), blurRadius = 0, color = Hex("#6C2843") };

            _soundButton = AddSquareButton(page, "♪", 38, 42, 48);
            _soundButton.clicked += ToggleSound;
            _helpButton = AddSquareButton(page, "?", 40, 964, 48);
            _helpButton.clicked += delegate { _rulesOverlay.style.display = DisplayStyle.Flex; };
        }

        private void BuildArena(VisualElement page)
        {
            VisualElement frameShadow = AddElement(page, "ArenaShadow");
            SetRect(frameShadow, 42, 232, 996, 1230);
            Round(frameShadow, 43);
            frameShadow.style.backgroundColor = Hex("#090B17");

            VisualElement outerFrame = AddElement(page, "ArenaFrame");
            SetRect(outerFrame, 42, 210, 996, 1230);
            Round(outerFrame, 43);
            outerFrame.style.backgroundColor = Hex("#9C6035");
            SetBorder(outerFrame, 8, Ink);

            VisualElement innerFrame = AddElement(outerFrame, "InnerFrame");
            SetRect(innerFrame, 18, 18, 944, 1184);
            Round(innerFrame, 30);
            innerFrame.style.backgroundColor = TableDark;
            SetBorder(innerFrame, 6, Hex("#D18A49"));
            innerFrame.style.overflow = Overflow.Hidden;

            VisualElement tableTopLight = AddElement(innerFrame, "TableTopLight");
            SetRect(tableTopLight, 28, 20, 888, 5);
            Round(tableTopLight, 3);
            tableTopLight.style.backgroundColor = new Color(1, 1, 1, .08f);

            _robotGlow = AddElement(innerFrame, "RobotGlow");
            SetRect(_robotGlow, 80, -80, 780, 240);
            Round(_robotGlow, 120);
            _robotGlow.style.backgroundColor = new Color(Red.r, Red.g, Red.b, .18f);
            _robotGlow.style.display = DisplayStyle.None;

            _playerGlow = AddElement(innerFrame, "PlayerGlow");
            SetRect(_playerGlow, 80, 1010, 780, 240);
            Round(_playerGlow, 120);
            _playerGlow.style.backgroundColor = new Color(Blue.r, Blue.g, Blue.b, .22f);
            _playerGlow.style.display = DisplayStyle.None;

            BuildRobotZone(innerFrame);
            BuildCenterZone(innerFrame);
            BuildPlayerZone(innerFrame);
            BuildDice3DLayer(innerFrame);

            AddFrameStud(outerFrame, 29, 29);
            AddFrameStud(outerFrame, 935, 29);
            AddFrameStud(outerFrame, 29, 1143);
            AddFrameStud(outerFrame, 935, 1143);
        }

        private void AddFrameStud(VisualElement parent, float x, float y)
        {
            VisualElement stud = AddElement(parent, "FrameStud");
            SetRect(stud, x, y, 26, 26);
            Round(stud, 13);
            stud.style.backgroundColor = Hex("#5E3829");
            SetBorder(stud, 3, Ink);
        }

        private void BuildDice3DLayer(VisualElement innerFrame)
        {
            _diceRenderTexture = new RenderTexture(944, 1184, 24, RenderTextureFormat.ARGB32)
            {
                name = "Dice21 3D Dice",
                antiAliasing = 4,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            _diceRenderTexture.Create();

            Image overlay = new Image
            {
                name = "Dice3DOverlay",
                image = _diceRenderTexture,
                scaleMode = ScaleMode.StretchToFill,
                pickingMode = PickingMode.Ignore
            };
            SetRect(overlay, 0, 0, 944, 1184);
            innerFrame.Add(overlay);
            overlay.BringToFront();

            _diceRenderRoot = new GameObject("Dice 3D Renderer");
            _diceRenderRoot.transform.SetParent(transform, false);

            GameObject cameraObject = new GameObject("Dice Render Camera");
            cameraObject.transform.SetParent(_diceRenderRoot.transform, false);
            cameraObject.transform.position = new Vector3(0, 0, -12);
            Camera diceCamera = cameraObject.AddComponent<Camera>();
            diceCamera.orthographic = true;
            diceCamera.orthographicSize = 5.92f;
            diceCamera.aspect = 944f / 1184f;
            diceCamera.clearFlags = CameraClearFlags.SolidColor;
            diceCamera.backgroundColor = new Color(0, 0, 0, 0);
            diceCamera.cullingMask = 1 << DiceRenderLayer;
            diceCamera.nearClipPlane = .1f;
            diceCamera.farClipPlane = 30f;
            diceCamera.allowHDR = false;
            diceCamera.allowMSAA = true;
            diceCamera.targetTexture = _diceRenderTexture;

            Camera mainCamera = Camera.main;
            if (mainCamera != null) mainCamera.cullingMask &= ~(1 << DiceRenderLayer);

            GameObject lightObject = new GameObject("Dice Key Light");
            lightObject.transform.SetParent(_diceRenderRoot.transform, false);
            lightObject.transform.rotation = Quaternion.Euler(35, -35, 0);
            Light keyLight = lightObject.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.color = new Color(1f, .93f, .82f);
            keyLight.intensity = 1.25f;
            keyLight.cullingMask = 1 << DiceRenderLayer;

            Material blueMaterial = CreateDiceBodyMaterial(new Color(.08f, .48f, .94f));
            Material redMaterial = CreateDiceBodyMaterial(new Color(.92f, .15f, .24f));
            Material pipMaterial = CreateUnlitMaterial(new Color(1f, .96f, .82f, 1f), false);

            Attach3DDie(_playerDice[0], "Player Die A", blueMaterial, pipMaterial, new Vector3(-.92f, -3.08f, 0), 3);
            Attach3DDie(_playerDice[1], "Player Die B", blueMaterial, pipMaterial, new Vector3(.98f, -3.08f, 0), 5);
            Attach3DDie(_robotDice[0], "Robot Die A", redMaterial, pipMaterial, new Vector3(-.92f, 3.62f, 0), 2);
            Attach3DDie(_robotDice[1], "Robot Die B", redMaterial, pipMaterial, new Vector3(.98f, 3.62f, 0), 4);
        }

        private void Attach3DDie(DieView die, string name, Material bodyMaterial, Material pipMaterial,
            Vector3 basePosition, int initialValue)
        {
            die.BaseWorldPosition = basePosition;
            die.Model3D = CreateDiceModel(name, bodyMaterial, pipMaterial);
            die.Model3D.transform.SetParent(_diceRenderRoot.transform, false);
            die.Model3D.transform.position = basePosition;

            die.Shadow3D = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            die.Shadow3D.name = name + " Shadow";
            die.Shadow3D.transform.SetParent(_diceRenderRoot.transform, false);
            die.Shadow3D.transform.position = basePosition + new Vector3(0, -.7f, .8f);
            die.Shadow3D.transform.localScale = new Vector3(1.12f, .18f, .08f);
            Destroy(die.Shadow3D.GetComponent<Collider>());
            die.ShadowRenderer3D = die.Shadow3D.GetComponent<Renderer>();
            die.ShadowRenderer3D.sharedMaterial = CreateUnlitMaterial(new Color(0, 0, 0, .43f), true);
            SetLayerRecursively(die.Shadow3D, DiceRenderLayer);

            die.Body.style.opacity = 0;
            die.Shadow.style.opacity = 0;
            HideGhosts(die);
            SetModelRest(die, initialValue);
        }

        private GameObject CreateDiceModel(string name, Material bodyMaterial, Material pipMaterial)
        {
            GameObject root = new GameObject(name);
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Body";
            body.transform.SetParent(root.transform, false);
            body.transform.localScale = Vector3.one * 1.14f;
            Destroy(body.GetComponent<Collider>());
            body.GetComponent<Renderer>().sharedMaterial = bodyMaterial;

            AddFacePips(root.transform, 1, Vector3.back, Vector3.right, Vector3.up, pipMaterial);
            AddFacePips(root.transform, 6, Vector3.forward, Vector3.left, Vector3.up, pipMaterial);
            AddFacePips(root.transform, 3, Vector3.right, Vector3.forward, Vector3.up, pipMaterial);
            AddFacePips(root.transform, 4, Vector3.left, Vector3.back, Vector3.up, pipMaterial);
            AddFacePips(root.transform, 2, Vector3.up, Vector3.right, Vector3.forward, pipMaterial);
            AddFacePips(root.transform, 5, Vector3.down, Vector3.right, Vector3.back, pipMaterial);
            SetLayerRecursively(root, DiceRenderLayer);
            return root;
        }

        private static void AddFacePips(Transform parent, int value, Vector3 normal, Vector3 right,
            Vector3 up, Material material)
        {
            Vector2[] positions;
            switch (value)
            {
                case 1: positions = new[] { Vector2.zero }; break;
                case 2: positions = new[] { new Vector2(-1, 1), new Vector2(1, -1) }; break;
                case 3: positions = new[] { new Vector2(-1, 1), Vector2.zero, new Vector2(1, -1) }; break;
                case 4: positions = new[] { new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1) }; break;
                case 5: positions = new[] { new Vector2(-1, 1), new Vector2(1, 1), Vector2.zero, new Vector2(-1, -1), new Vector2(1, -1) }; break;
                default: positions = new[] { new Vector2(-1, 1), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1) }; break;
            }

            foreach (Vector2 position in positions)
            {
                GameObject pip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                pip.name = "Pip";
                pip.transform.SetParent(parent, false);
                pip.transform.localPosition = normal * .585f + right * (position.x * .245f) + up * (position.y * .245f);
                pip.transform.localScale = Vector3.one * .145f;
                Destroy(pip.GetComponent<Collider>());
                pip.GetComponent<Renderer>().sharedMaterial = material;
            }
        }

        private static Material CreateDiceBodyMaterial(Color color)
        {
            Shader shader = Resources.Load<Shader>("DiceLit") ?? Shader.Find("Sprites/Default");
            if (shader == null) throw new InvalidOperationException("Dice shader was not included in the player build.");
            Material material = new Material(shader) { color = color };
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            return material;
        }

        private static Material CreateUnlitMaterial(Color color, bool transparent)
        {
            Shader shader = Resources.Load<Shader>(transparent ? "DiceTransparent" : "DiceLit") ?? Shader.Find("Sprites/Default");
            if (shader == null) throw new InvalidOperationException("Dice shader was not included in the player build.");
            Material material = new Material(shader) { color = color };
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            return material;
        }

        private static void SetLayerRecursively(GameObject target, int layer)
        {
            target.layer = layer;
            foreach (Transform child in target.transform) SetLayerRecursively(child.gameObject, layer);
        }

        private void BuildRobotZone(VisualElement parent)
        {
            VisualElement divider = AddElement(parent, "TopDivider");
            SetRect(divider, 35, 360, 874, 3);
            divider.style.backgroundColor = new Color(1, 1, 1, .08f);

            AddScorePanel(parent, 332, 42, 280, 96, "机器人", Red, out _robotScoreLabel);

            VisualElement diceRow = AddElement(parent, "RobotDiceRow");
            SetRect(diceRow, 305, 155, 340, 165);
            diceRow.style.flexDirection = FlexDirection.Row;
            diceRow.style.justifyContent = Justify.SpaceBetween;
            diceRow.style.alignItems = Align.Center;
            _robotDice[0] = CreateDie(diceRow, Hex("#FF5C68"), 2);
            _robotDice[1] = CreateDie(diceRow, Hex("#FF5C68"), 4);
        }

        private void BuildCenterZone(VisualElement parent)
        {
            VisualElement target = AddElement(parent, "TargetMark");
            SetRect(target, 300, 410, 340, 280);
            target.style.opacity = .05f;
            target.style.rotate = new Rotate(-7);
            Label targetNumber = AddLabel(target, "21", 210, Cream, FontStyle.Bold);
            SetRect(targetNumber, 0, 0, 340, 220);

            VisualElement bubbleShadow = AddElement(parent, "BubbleShadow");
            SetRect(bubbleShadow, 170, 483, 604, 128);
            Round(bubbleShadow, 26);
            bubbleShadow.style.backgroundColor = new Color(0, 0, 0, .3f);

            VisualElement bubble = AddElement(parent, "MessageBubble");
            SetRect(bubble, 170, 473, 604, 128);
            Round(bubble, 26);
            bubble.style.backgroundColor = Cream;
            SetBorder(bubble, 4, Ink);
            _messageKicker = AddLabel(bubble, "", 1, Hex("#A63D4D"), FontStyle.Normal);
            SetRect(_messageKicker, 0, 0, 1, 1);
            _messageKicker.style.display = DisplayStyle.None;
            _messageLabel = AddLabel(bubble, "投骰子，别超过 21", 32, Hex("#262030"), FontStyle.Bold);
            SetRect(_messageLabel, 38, 20, 528, 88);
            _messageLabel.style.whiteSpace = WhiteSpace.Normal;

            VisualElement bubbleAccent = AddElement(bubble, "BubbleAccent");
            SetRect(bubbleAccent, 17, 22, 7, 84);
            Round(bubbleAccent, 4);
            bubbleAccent.style.backgroundColor = Gold;

            VisualElement historyShelf = AddElement(parent, "HistoryShelf");
            SetRect(historyShelf, 0, 0, 1, 1);
            historyShelf.style.display = DisplayStyle.None;

            _historyEmptyLabel = AddLabel(historyShelf, "", 1, Color.clear, FontStyle.Normal);
            SetRect(_historyEmptyLabel, 0, 0, 1, 1);

            _historyRow = AddElement(historyShelf, "History");
            SetRect(_historyRow, 0, 0, 1, 1);
            _historyRow.style.flexDirection = FlexDirection.Row;
            _historyRow.style.justifyContent = Justify.Center;
            _historyRow.style.alignItems = Align.Center;

            VisualElement middleDivider = AddElement(parent, "MiddleDivider");
            SetRect(middleDivider, 35, 780, 874, 3);
            middleDivider.style.backgroundColor = new Color(1, 1, 1, .08f);
        }

        private void BuildPlayerZone(VisualElement parent)
        {
            VisualElement diceRow = AddElement(parent, "PlayerDiceRow");
            SetRect(diceRow, 305, 820, 340, 170);
            diceRow.style.flexDirection = FlexDirection.Row;
            diceRow.style.justifyContent = Justify.SpaceBetween;
            diceRow.style.alignItems = Align.Center;
            _playerDice[0] = CreateDie(diceRow, Hex("#32A7FF"), 3);
            _playerDice[1] = CreateDie(diceRow, Hex("#32A7FF"), 5);

            AddScorePanel(parent, 332, 1024, 280, 96, "你", Blue, out _playerScoreLabel);

            _statusLabel = AddLabel(parent, "●  你的回合", 23, Hex("#A5A0B2"), FontStyle.Bold);
            SetRect(_statusLabel, 0, 0, 1, 1);
            _statusLabel.style.color = Blue;
            _statusLabel.style.display = DisplayStyle.None;
        }

        private VisualElement AddScorePanel(VisualElement parent, float x, float y, float width, float height,
            string owner, Color accent, out Label scoreLabel)
        {
            VisualElement shadow = AddElement(parent, owner + "ScoreShadow");
            SetRect(shadow, x, y + 7, width, height);
            Round(shadow, 23);
            shadow.style.backgroundColor = new Color(0, 0, 0, .38f);

            VisualElement panel = AddElement(parent, owner + "ScorePanel");
            SetRect(panel, x, y, width, height);
            Round(panel, 23);
            panel.style.backgroundColor = new Color(.035f, .055f, .09f, .78f);
            SetBorder(panel, 4, new Color(0, 0, 0, .6f));

            VisualElement accentBar = AddElement(panel, "ScoreAccent");
            SetRect(accentBar, 0, 17, 7, height - 34);
            Round(accentBar, 4);
            accentBar.style.backgroundColor = accent;

            Label ownerLabel = AddLabel(panel, owner, 22, Hex("#C5BEC9"), FontStyle.Bold);
            SetRect(ownerLabel, 20, 0, width * .34f, height);
            Label slash = AddLabel(panel, "/ 21", 30, Hex("#9994A4"), FontStyle.Normal);
            SetRect(slash, width - 86, 0, 78, height);
            scoreLabel = AddLabel(panel, "0", 68, accent, FontStyle.Bold);
            SetRect(scoreLabel, width * .37f, 0, width * .35f, height);
            return panel;
        }

        private DieView CreateDie(VisualElement parent, Color tint, int initialValue)
        {
            VisualElement root = AddElement(parent, "Die");
            root.style.width = 150;
            root.style.height = 150;
            root.style.position = Position.Relative;
            root.style.overflow = Overflow.Visible;

            VisualElement shadow = AddElement(root, "DieShadow");
            SetRect(shadow, 20, 122, 110, 25);
            Round(shadow, 50);
            shadow.style.backgroundColor = new Color(0, 0, 0, .42f);

            Image[] ghosts = new Image[1];
            for (int i = ghosts.Length - 1; i >= 0; i--)
            {
                Image ghost = new Image();
                ghost.name = "DieTrail" + i;
                ghost.scaleMode = ScaleMode.ScaleToFit;
                ghost.tintColor = new Color(tint.r, tint.g, tint.b, .72f);
                ghost.style.opacity = 0;
                SetRect(ghost, 4, 0, 142, 142);
                root.Add(ghost);
                ghosts[i] = ghost;
            }

            VisualElement body = AddElement(root, "DieBody");
            SetRect(body, 4, 0, 142, 142);
            body.style.overflow = Overflow.Visible;

            Image art = new Image();
            art.name = "DieArt";
            art.scaleMode = ScaleMode.ScaleToFit;
            art.tintColor = tint;
            SetRect(art, 0, 0, 142, 142);
            body.Add(art);

            DieView die = new DieView
            {
                Root = root,
                Shadow = shadow,
                Body = body,
                Art = art,
                Ghosts = ghosts,
                Tint = tint,
                Value = initialValue
            };
            SetDieResult(die, initialValue, 0);
            ResetDiePose(die);
            return die;
        }

        private void BuildControls(VisualElement page)
        {
            VisualElement dockShadow = AddElement(page, "ControlDockShadow");
            SetRect(dockShadow, 33, 1484, 1014, 218);
            Round(dockShadow, 34);
            dockShadow.style.backgroundColor = new Color(0, 0, 0, .4f);

            VisualElement dock = AddElement(page, "ControlDock");
            SetRect(dock, 33, 1470, 1014, 218);
            Round(dock, 34);
            dock.style.backgroundColor = new Color(.12f, .105f, .2f, .96f);
            SetBorder(dock, 5, Ink);

            _holdButton = AddActionButton(page, "保存", 53, 1504, 455, 150, _redButton);
            _rollButton = AddActionButton(page, "投骰子", 520, 1504, 507, 150, _yellowButton);
            _holdButton.clicked += HoldClicked;
            _rollButton.clicked += RollClicked;

            _tipLabel = AddLabel(page, "", 1, Color.clear, FontStyle.Normal);
            SetRect(_tipLabel, 0, 0, 1, 1);
            _tipLabel.style.display = DisplayStyle.None;
        }

        private void BuildRulesOverlay(VisualElement page)
        {
            _rulesOverlay = AddElement(page, "RulesOverlay");
            Stretch(_rulesOverlay);
            _rulesOverlay.style.backgroundColor = new Color(.02f, .02f, .06f, .86f);
            _rulesOverlay.style.alignItems = Align.Center;
            _rulesOverlay.style.justifyContent = Justify.Center;

            VisualElement card = AddElement(_rulesOverlay, "RulesCard");
            card.style.width = 850;
            card.style.height = 820;
            card.style.backgroundColor = Panel;
            Round(card, 40);
            SetBorder(card, 6, Ink);

            Label eyebrow = AddLabel(card, "HOW TO PLAY", 22, Gold, FontStyle.Bold);
            SetRect(eyebrow, 80, 62, 690, 38);
            Label title = AddLabel(card, "向 21 靠近，及时停手", 48, Cream, FontStyle.Bold);
            SetRect(title, 70, 110, 710, 80);

            AddRule(card, 1, "每次同时投掷两颗骰子，点数会累加。", 220);
            AddRule(card, 2, "没有超过 21 时，可以继续投，也可以保存点数。", 350);
            AddRule(card, 3, "机器人会不断投掷，直到超过你的点数获胜，或超过 21 落败。", 480);

            Button understood = AddActionButton(card, "明白了", 145, 680, 560, 95, _yellowButton);
            understood.clicked += delegate { _rulesOverlay.style.display = DisplayStyle.None; };
            _rulesOverlay.style.display = DisplayStyle.None;
        }

        private void AddRule(VisualElement card, int number, string text, float top)
        {
            Label numberLabel = AddLabel(card, number.ToString(), 30, Ink, FontStyle.Bold);
            SetRect(numberLabel, 75, top, 62, 62);
            numberLabel.style.backgroundColor = Gold;
            Round(numberLabel, 18);
            Label textLabel = AddLabel(card, text, 27, Hex("#D0C9D8"), FontStyle.Normal);
            SetRect(textLabel, 165, top - 5, 610, 90);
            textLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            textLabel.style.whiteSpace = WhiteSpace.Normal;
        }

        private void BuildResultOverlay(VisualElement page)
        {
            _resultOverlay = AddElement(page, "ResultOverlay");
            Stretch(_resultOverlay);
            _resultOverlay.style.backgroundColor = new Color(.02f, .02f, .06f, .9f);
            _resultOverlay.style.alignItems = Align.Center;
            _resultOverlay.style.justifyContent = Justify.Center;

            _resultCard = AddElement(_resultOverlay, "ResultCard");
            _resultCard.style.width = 800;
            _resultCard.style.height = 720;
            _resultCard.style.backgroundColor = Panel;
            Round(_resultCard, 44);
            SetBorder(_resultCard, 6, Ink);

            _resultBadge = AddLabel(_resultCard, "胜", 86, Hex("#5C3610"), FontStyle.Bold);
            SetRect(_resultBadge, 300, -85, 200, 200);
            Round(_resultBadge, 100);
            _resultBadge.style.backgroundColor = Gold;
            SetBorder(_resultBadge, 8, Hex("#F6DC8D"));

            _resultKicker = AddLabel(_resultCard, "LUCKY!", 22, Gold, FontStyle.Bold);
            SetRect(_resultKicker, 100, 140, 600, 36);
            _resultTitle = AddLabel(_resultCard, "你赢了！", 62, Cream, FontStyle.Bold);
            SetRect(_resultTitle, 80, 180, 640, 90);
            _resultDescription = AddLabel(_resultCard, "机器人超过了 21。", 25, Muted, FontStyle.Normal);
            SetRect(_resultDescription, 70, 275, 660, 82);
            _resultDescription.style.whiteSpace = WhiteSpace.Normal;

            VisualElement scores = AddElement(_resultCard, "ResultScores");
            SetRect(scores, 85, 380, 630, 130);
            Round(scores, 25);
            scores.style.backgroundColor = new Color(.02f, .03f, .08f, .48f);
            Label you = AddLabel(scores, "你的点数", 19, Muted, FontStyle.Normal);
            SetRect(you, 30, 15, 260, 35);
            _resultPlayerScore = AddLabel(scores, "0", 55, Cream, FontStyle.Bold);
            SetRect(_resultPlayerScore, 30, 50, 260, 70);
            Label robot = AddLabel(scores, "机器人点数", 19, Muted, FontStyle.Normal);
            SetRect(robot, 340, 15, 260, 35);
            _resultRobotScore = AddLabel(scores, "0", 55, Cream, FontStyle.Bold);
            SetRect(_resultRobotScore, 340, 50, 260, 70);
            VisualElement scoreDivider = AddElement(scores, "ScoreDivider");
            SetRect(scoreDivider, 314, 20, 2, 90);
            scoreDivider.style.backgroundColor = new Color(1, 1, 1, .1f);

            Button restart = AddActionButton(_resultCard, "再来一局", 120, 565, 560, 100, _yellowButton);
            restart.clicked += ResetGame;
            _resultOverlay.style.display = DisplayStyle.None;
        }

        private Button AddSquareButton(VisualElement parent, string text, int size, float x, float y)
        {
            Button button = new Button();
            button.text = text;
            button.style.position = Position.Absolute;
            button.style.left = x;
            button.style.top = y;
            button.style.width = 74;
            button.style.height = 74;
            button.style.fontSize = size;
            button.style.color = Cream;
            button.style.backgroundColor = Hex("#353149");
            button.style.unityFont = _font;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            Round(button, 20);
            SetBorder(button, 4, Hex("#4C4769"));
            parent.Add(button);
            return button;
        }

        private Button AddActionButton(VisualElement parent, string text, float x, float y, float width, float height, Texture2D texture)
        {
            Button button = new Button();
            button.text = text;
            button.style.position = Position.Absolute;
            button.style.left = x;
            button.style.top = y;
            button.style.width = width;
            button.style.height = height;
            button.style.fontSize = Mathf.Min(43, height * .31f);
            button.style.color = Cream;
            button.style.unityFont = _font;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            button.style.whiteSpace = WhiteSpace.Normal;
            button.style.backgroundColor = Color.white;
            if (texture != null)
            {
                button.style.backgroundImage = new StyleBackground(texture);
                button.style.unityBackgroundScaleMode = ScaleMode.StretchToFill;
            }
            Round(button, 25);
            SetBorder(button, 5, Ink);
            parent.Add(button);
            return button;
        }

        private void BuildAudio()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _rollClip = CreateNoiseClip("DiceRoll", .95f, .13f);
            _impactClip = CreateImpactClip();
            _scoreClip = CreateToneClip("Score", new[] { 440f, 610f }, .1f);
            _winClip = CreateToneClip("Win", new[] { 523f, 659f, 784f, 1046f }, .13f);
            _loseClip = CreateToneClip("Lose", new[] { 330f, 277f, 220f }, .17f);
        }

        private static AudioClip CreateNoiseClip(string name, float duration, float volume)
        {
            int sampleRate = 22050;
            int samples = Mathf.CeilToInt(duration * sampleRate);
            float[] data = new float[samples];
            System.Random random = new System.Random(21);
            for (int i = 0; i < samples; i++)
            {
                float envelope = Mathf.Exp(-3.2f * i / samples);
                float pulse = (i % 240 < 30) ? 1f : .25f;
                data[i] = ((float)random.NextDouble() * 2f - 1f) * volume * envelope * pulse;
            }
            AudioClip clip = AudioClip.Create(name, samples, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateToneClip(string name, float[] frequencies, float noteDuration)
        {
            int sampleRate = 22050;
            int samplesPerNote = Mathf.CeilToInt(noteDuration * sampleRate);
            float[] data = new float[samplesPerNote * frequencies.Length];
            for (int note = 0; note < frequencies.Length; note++)
            {
                for (int i = 0; i < samplesPerNote; i++)
                {
                    float t = i / (float)sampleRate;
                    float envelope = Mathf.Sin(Mathf.PI * i / samplesPerNote);
                    data[note * samplesPerNote + i] = Mathf.Sin(t * frequencies[note] * Mathf.PI * 2f) * envelope * .12f;
                }
            }
            AudioClip clip = AudioClip.Create(name, data.Length, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateImpactClip()
        {
            const int sampleRate = 22050;
            const float duration = .18f;
            int samples = Mathf.CeilToInt(duration * sampleRate);
            float[] data = new float[samples];
            System.Random random = new System.Random(2107);
            for (int i = 0; i < samples; i++)
            {
                float t = i / (float)sampleRate;
                float envelope = Mathf.Exp(-24f * t);
                float lowTone = Mathf.Sin(t * 105f * Mathf.PI * 2f) * .65f;
                float click = ((float)random.NextDouble() * 2f - 1f) * .35f;
                data[i] = (lowTone + click) * envelope * .22f;
            }
            AudioClip clip = AudioClip.Create("DiceImpact", samples, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private void Play(AudioClip clip)
        {
            if (_soundOn && clip != null) _audioSource.PlayOneShot(clip);
        }

        private void ToggleSound()
        {
            _soundOn = !_soundOn;
            _soundButton.text = _soundOn ? "♪" : "×";
            if (_soundOn) Play(_scoreClip);
        }

        private void RollClicked()
        {
            if (_busy || _gameOver) return;
            StartCoroutine(PlayerRollRoutine());
        }

        private IEnumerator PlayerRollRoutine()
        {
            _busy = true;
            UpdateButtons();
            SetMessage("", "投掷中…", false);
            int[] values = { UnityEngine.Random.Range(1, 7), UnityEngine.Random.Range(1, 7) };
            yield return AnimateDicePair(_playerDice, values);

            int gained = values[0] + values[1];
            _playerScore += gained;
            _playerHasRolled = true;
            AddHistory("你", values, Blue);
            yield return PopScore(_playerScoreLabel, _playerScore);
            Play(_scoreClip);

            if (_playerScore > 21)
            {
                SetMessage("", "超过 21", true);
                _tipLabel.text = _playerScore + " 点已经超过 21，本局结束";
                yield return ShakeArena(.4f);
                EndGame(false, "你超过了 21，机器人获胜。");
                yield break;
            }

            if (_playerScore == 21)
            {
                SetMessage("", "正好 21！", false);
                _tipLabel.text = "正好 21！保存后机器人无法用有效点数超过你";
            }
            else
            {
                SetMessage("", "继续投，还是保存？", false);
                _tipLabel.text = "当前 " + _playerScore + " 点，距离 21 还差 " + (21 - _playerScore) + " 点";
            }

            _busy = false;
            UpdateButtons();
        }

        private void HoldClicked()
        {
            if (_busy || _gameOver || !_playerHasRolled) return;
            StartCoroutine(RobotTurnRoutine());
        }

        private IEnumerator RobotTurnRoutine()
        {
            _busy = true;
            _robotGlow.style.opacity = 1;
            _playerGlow.style.opacity = .2f;
            _statusLabel.text = "●  机器人思考中";
            _statusLabel.style.color = Red;
            UpdateButtons();
            SetMessage("", "机器人开始投骰", true);
            _tipLabel.text = "机器人不会停手：超过你的点数就赢，超过 21 就输";
            yield return new WaitForSeconds(.75f);

            while (!_gameOver)
            {
                SetMessage("", "机器人继续投骰", true);
                yield return new WaitForSeconds(.45f);
                int[] values = { UnityEngine.Random.Range(1, 7), UnityEngine.Random.Range(1, 7) };
                SetMessage("", "机器人投掷中…", true);
                yield return AnimateDicePair(_robotDice, values);

                int gained = values[0] + values[1];
                _robotScore += gained;
                AddHistory("机", values, Red);
                yield return PopScore(_robotScoreLabel, _robotScore);
                Play(_scoreClip);

                if (_robotScore > 21)
                {
                    SetMessage("", "机器人超过 21", false);
                    yield return ShakeArena(.4f);
                    EndGame(true, "机器人没能及时停手，最终超过了 21。");
                    yield break;
                }

                if (_robotScore > _playerScore)
                {
                    SetMessage("", "机器人领先", true);
                    yield return new WaitForSeconds(.65f);
                    EndGame(false, "机器人没有超过 21，并且点数已经大于你。");
                    yield break;
                }

                SetMessage("", _robotScore == _playerScore ? "平局，机器人继续" : "机器人继续投骰", true);
                yield return new WaitForSeconds(.65f);
            }
        }

        private IEnumerator AnimateDicePair(DieView[] pair, int[] values)
        {
            if (pair[0].Model3D != null && pair[1].Model3D != null)
            {
                yield return Animate3DDicePair(pair, values);
                yield break;
            }

            Play(_rollClip);

            // Wind-up: the dice compress into the table before they are released.
            const float windUpDuration = .14f;
            float elapsed = 0;
            while (elapsed < windUpDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Smooth01(elapsed / windUpDuration);
                for (int i = 0; i < pair.Length; i++)
                {
                    float direction = i == 0 ? -1f : 1f;
                    SetBodyPose(pair[i], 0, Mathf.Lerp(0, 11, progress), direction * Mathf.Lerp(0, 7, progress),
                        Mathf.Lerp(1f, 1.13f, progress), Mathf.Lerp(1f, .76f, progress));
                    SetShadowPose(pair[i], 0, Mathf.Lerp(1f, 1.25f, progress), Mathf.Lerp(1f, .72f, progress),
                        Mathf.Lerp(.42f, .72f, progress));
                }
                yield return null;
            }

            // Flight: each die follows a clean ballistic arc. The translucent copies are
            // sampled from earlier points on the same curve, producing readable motion blur.
            const float flightDuration = .74f;
            elapsed = 0;
            while (elapsed < flightDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / flightDuration);
                for (int i = 0; i < pair.Length; i++)
                {
                    float delayed = i * .055f;
                    float dieProgress = Mathf.Clamp01((progress - delayed) / (1f - delayed));
                    SetRollingFace(pair[i], elapsed, i);

                    CalculateFlightPose(dieProgress, i, out float x, out float y, out float rotation, out float scale);
                    float stretch = Mathf.Sin(dieProgress * Mathf.PI * 2f + i) * .045f;
                    SetBodyPose(pair[i], x, y, rotation, scale + stretch, scale - stretch);

                    float height = Mathf.Sin(dieProgress * Mathf.PI);
                    SetShadowPose(pair[i], x, Mathf.Lerp(1.18f, .48f, height), Mathf.Lerp(.72f, .42f, height),
                        Mathf.Lerp(.62f, .16f, height));
                    UpdateGhostTrail(pair[i], dieProgress, i);
                }
                yield return null;
            }

            // Hard first contact. Faces lock exactly at impact, followed by two smaller
            // bounces so the stop feels physical instead of being a sudden sprite swap.
            for (int i = 0; i < pair.Length; i++)
            {
                pair[i].Value = values[i];
                SetDieResult(pair[i], values[i], 1);
                HideGhosts(pair[i]);
                float direction = i == 0 ? -1f : 1f;
                SetBodyPose(pair[i], direction * 18f, 2, direction * 20f, 1.2f, .72f);
                SetShadowPose(pair[i], direction * 18f, 1.36f, .58f, .78f);
            }

            EmitLandingBurst(pair[0], pair[0].Tint);
            EmitLandingBurst(pair[1], pair[1].Tint);
            Play(_impactClip);
            yield return new WaitForSeconds(.055f);

            const float firstBounceDuration = .24f;
            elapsed = 0;
            while (elapsed < firstBounceDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / firstBounceDuration);
                float height = Mathf.Sin(progress * Mathf.PI);
                float eased = EaseOutCubic(progress);
                for (int i = 0; i < pair.Length; i++)
                {
                    float direction = i == 0 ? -1f : 1f;
                    float x = Mathf.Lerp(direction * 18f, direction * 7f, eased);
                    float y = -42f * height;
                    float rotation = Mathf.Lerp(direction * 20f, -direction * 7f, eased);
                    float scaleX = 1f + .2f * (1f - progress) - .07f * height;
                    float scaleY = 1f - .28f * (1f - progress) + .1f * height;
                    SetBodyPose(pair[i], x, y, rotation, scaleX, scaleY);
                    SetShadowPose(pair[i], x, Mathf.Lerp(1.28f, .74f, height), Mathf.Lerp(.58f, .45f, height),
                        Mathf.Lerp(.72f, .3f, height));
                }
                yield return null;
            }

            for (int i = 0; i < pair.Length; i++) SetDieResult(pair[i], values[i], 0);

            const float secondBounceDuration = .18f;
            elapsed = 0;
            while (elapsed < secondBounceDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / secondBounceDuration);
                float height = Mathf.Sin(progress * Mathf.PI);
                float eased = EaseOutCubic(progress);
                for (int i = 0; i < pair.Length; i++)
                {
                    float direction = i == 0 ? -1f : 1f;
                    float x = Mathf.Lerp(direction * 7f, 0, eased);
                    float y = -13f * height;
                    float rotation = Mathf.Lerp(-direction * 7f, 0, eased);
                    float wobble = Mathf.Sin(progress * Mathf.PI * 2f) * .035f * (1f - progress);
                    SetBodyPose(pair[i], x, y, rotation, 1f + wobble, 1f - wobble);
                    SetShadowPose(pair[i], x, Mathf.Lerp(1.08f, .9f, height), Mathf.Lerp(.58f, .5f, height),
                        Mathf.Lerp(.55f, .38f, height));
                }
                yield return null;
            }

            for (int i = 0; i < pair.Length; i++) ResetDiePose(pair[i]);
        }

        private IEnumerator Animate3DDicePair(DieView[] pair, int[] values)
        {
            Play(_rollClip);
            bool robotPair = ReferenceEquals(pair, _robotDice);
            Quaternion[] startRotations = { pair[0].Model3D.transform.rotation, pair[1].Model3D.transform.rotation };
            Quaternion[] finalRotations = { GetRestRotation(values[0]), GetRestRotation(values[1]) };

            const float windUpDuration = .14f;
            float elapsed = 0;
            while (elapsed < windUpDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Smooth01(elapsed / windUpDuration);
                for (int i = 0; i < pair.Length; i++)
                {
                    float direction = i == 0 ? -1f : 1f;
                    Vector3 position = pair[i].BaseWorldPosition + new Vector3(0, -.11f * progress, 0);
                    Quaternion rotation = startRotations[i] * Quaternion.Euler(0, 0, direction * 8f * progress);
                    Vector3 scale = new Vector3(Mathf.Lerp(1f, 1.1f, progress), Mathf.Lerp(1f, .72f, progress), Mathf.Lerp(1f, 1.1f, progress));
                    SetModelPose(pair[i], position, rotation, scale);
                    SetModelShadowPose(pair[i], 0, 1.24f, .72f, Mathf.Lerp(.43f, .62f, progress));
                }
                yield return null;
            }

            const float flightDuration = .82f;
            elapsed = 0;
            while (elapsed < flightDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / flightDuration);
                for (int i = 0; i < pair.Length; i++)
                {
                    float delay = i * .05f;
                    float dieProgress = Mathf.Clamp01((progress - delay) / (1f - delay));
                    float direction = i == 0 ? -1f : 1f;
                    float height = Mathf.Sin(dieProgress * Mathf.PI);
                    float flutter = Mathf.Sin(dieProgress * Mathf.PI * 3f + i * 1.2f) * height * .07f;
                    float x = direction * (.52f * height + .15f * dieProgress) + flutter;
                    float arc = robotPair ? (-.86f - i * .05f) : (1.58f + i * .08f);
                    float y = -.11f * (1f - dieProgress) + height * arc;
                    Vector3 position = pair[i].BaseWorldPosition + new Vector3(x, y, -height * .45f);

                    Quaternion spin = startRotations[i] * Quaternion.Euler(
                        direction * 760f * dieProgress,
                        (i == 0 ? 610f : -670f) * dieProgress,
                        direction * 330f * dieProgress);
                    float lockProgress = Smooth01((dieProgress - .7f) / .3f);
                    Quaternion rotation = Quaternion.Slerp(spin, finalRotations[i], lockProgress);
                    float scale = 1f + height * .11f;
                    SetModelPose(pair[i], position, rotation, Vector3.one * scale);
                    SetModelShadowPose(pair[i], x, Mathf.Lerp(1.18f, .5f, height), Mathf.Lerp(.18f, .1f, height),
                        Mathf.Lerp(.48f, .14f, height));
                }
                yield return null;
            }

            for (int i = 0; i < pair.Length; i++)
            {
                float direction = i == 0 ? -1f : 1f;
                pair[i].Value = values[i];
                SetDieResult(pair[i], values[i], 1);
                Vector3 impactPosition = pair[i].BaseWorldPosition + new Vector3(direction * .18f, -.02f, 0);
                Quaternion impactRotation = finalRotations[i] * Quaternion.Euler(0, 0, direction * 10f);
                SetModelPose(pair[i], impactPosition, impactRotation, new Vector3(1.18f, .72f, 1.18f));
                SetModelShadowPose(pair[i], direction * .18f, 1.34f, .2f, .65f);
            }

            EmitLandingBurst(pair[0], pair[0].Tint);
            EmitLandingBurst(pair[1], pair[1].Tint);
            Play(_impactClip);
            yield return new WaitForSeconds(.055f);

            const float firstBounceDuration = .25f;
            elapsed = 0;
            while (elapsed < firstBounceDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / firstBounceDuration);
                float height = Mathf.Sin(progress * Mathf.PI);
                float eased = EaseOutCubic(progress);
                for (int i = 0; i < pair.Length; i++)
                {
                    float direction = i == 0 ? -1f : 1f;
                    float x = Mathf.Lerp(direction * .18f, direction * .07f, eased);
                    Vector3 position = pair[i].BaseWorldPosition + new Vector3(x, height * .42f, -height * .08f);
                    Quaternion impactRotation = finalRotations[i] * Quaternion.Euler(0, 0, direction * 10f);
                    Quaternion rotation = Quaternion.Slerp(impactRotation, finalRotations[i], eased);
                    Vector3 scale = new Vector3(
                        1f + .18f * (1f - progress) - .06f * height,
                        1f - .28f * (1f - progress) + .1f * height,
                        1f + .18f * (1f - progress) - .06f * height);
                    SetModelPose(pair[i], position, rotation, scale);
                    SetModelShadowPose(pair[i], x, Mathf.Lerp(1.26f, .72f, height), Mathf.Lerp(.2f, .12f, height),
                        Mathf.Lerp(.58f, .25f, height));
                }
                yield return null;
            }

            for (int i = 0; i < pair.Length; i++) SetDieResult(pair[i], values[i], 0);

            const float secondBounceDuration = .18f;
            elapsed = 0;
            while (elapsed < secondBounceDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / secondBounceDuration);
                float height = Mathf.Sin(progress * Mathf.PI);
                float eased = EaseOutCubic(progress);
                for (int i = 0; i < pair.Length; i++)
                {
                    float direction = i == 0 ? -1f : 1f;
                    float x = Mathf.Lerp(direction * .07f, 0, eased);
                    Vector3 position = pair[i].BaseWorldPosition + new Vector3(x, height * .13f, 0);
                    Quaternion wobble = finalRotations[i] * Quaternion.Euler(0, 0, direction * Mathf.Sin(progress * Mathf.PI) * 3f);
                    float scaleOffset = Mathf.Sin(progress * Mathf.PI * 2f) * .025f * (1f - progress);
                    SetModelPose(pair[i], position, wobble, new Vector3(1f + scaleOffset, 1f - scaleOffset, 1f + scaleOffset));
                    SetModelShadowPose(pair[i], x, Mathf.Lerp(1.08f, .88f, height), Mathf.Lerp(.18f, .14f, height),
                        Mathf.Lerp(.43f, .3f, height));
                }
                yield return null;
            }

            for (int i = 0; i < pair.Length; i++) SetModelRest(pair[i], values[i]);
        }

        private static void SetModelPose(DieView die, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            die.Model3D.transform.position = position;
            die.Model3D.transform.rotation = rotation;
            die.Model3D.transform.localScale = scale;
        }

        private static void SetModelShadowPose(DieView die, float xOffset, float scaleX, float scaleY, float opacity)
        {
            die.Shadow3D.transform.position = die.BaseWorldPosition + new Vector3(xOffset, -.7f, .8f);
            die.Shadow3D.transform.localScale = new Vector3(scaleX, scaleY, .08f);
            if (die.ShadowRenderer3D != null)
            {
                Color color = die.ShadowRenderer3D.material.color;
                color.a = opacity;
                die.ShadowRenderer3D.material.color = color;
            }
        }

        private static void SetModelRest(DieView die, int value)
        {
            if (die.Model3D == null) return;
            die.Model3D.SetActive(true);
            die.Shadow3D.SetActive(true);
            SetModelPose(die, die.BaseWorldPosition, GetRestRotation(value), Vector3.one);
            SetModelShadowPose(die, 0, 1.12f, .18f, .43f);
        }

        private static Quaternion GetRestRotation(int value)
        {
            Quaternion faceRotation;
            switch (value)
            {
                case 1: faceRotation = Quaternion.identity; break;
                case 2: faceRotation = Quaternion.Euler(-90, 0, 0); break;
                case 3: faceRotation = Quaternion.Euler(0, 90, 0); break;
                case 4: faceRotation = Quaternion.Euler(0, -90, 0); break;
                case 5: faceRotation = Quaternion.Euler(90, 0, 0); break;
                default: faceRotation = Quaternion.Euler(0, 180, 0); break;
            }
            return Quaternion.Euler(-14, 20, -4) * faceRotation;
        }

        private void SetRollingFace(DieView die, float elapsed, int dieIndex)
        {
            Sprite[] source = _rollSprites.Length > 0 ? _rollSprites : _resultSprites;
            if (source.Length == 0) return;
            int index = (Mathf.FloorToInt(elapsed * 16f) + dieIndex * 5) % source.Length;
            Sprite sprite = source[index];
            die.Art.sprite = sprite;
            foreach (Image ghost in die.Ghosts) ghost.sprite = sprite;
        }

        private static void CalculateFlightPose(float progress, int dieIndex,
            out float x, out float y, out float rotation, out float scale)
        {
            float direction = dieIndex == 0 ? -1f : 1f;
            float height = Mathf.Sin(progress * Mathf.PI);
            float sidewaysFlutter = Mathf.Sin(progress * Mathf.PI * 3f + dieIndex * 1.2f) * height * 7f;
            x = direction * (64f * height + 18f * progress) + sidewaysFlutter;
            y = 11f * (1f - progress) - height * (172f + dieIndex * 10f);
            // The sprite sequence already contains perspective changes. A restrained
            // rocking angle keeps those 3D silhouettes readable instead of smearing them.
            rotation = direction * (7f + 24f * Mathf.Sin(progress * Mathf.PI * 3f) + 11f * progress);
            scale = 1f + height * .27f;
        }

        private void UpdateGhostTrail(DieView die, float progress, int dieIndex)
        {
            float visibility = Mathf.Sin(Mathf.Clamp01(progress / .92f) * Mathf.PI);
            for (int i = 0; i < die.Ghosts.Length; i++)
            {
                float ghostProgress = Mathf.Clamp01(progress - (i + 1) * .035f);
                CalculateFlightPose(ghostProgress, dieIndex, out float x, out float y, out float rotation, out float scale);
                Image ghost = die.Ghosts[i];
                ghost.style.translate = new Translate(x, y, 0);
                ghost.style.rotate = new Rotate(rotation);
                ghost.style.scale = new Scale(new Vector3(scale, scale, 1));
                ghost.style.opacity = visibility * .11f;
            }
        }

        private static void SetBodyPose(DieView die, float x, float y, float rotation, float scaleX, float scaleY)
        {
            die.Body.style.translate = new Translate(x, y, 0);
            die.Body.style.rotate = new Rotate(rotation);
            die.Body.style.scale = new Scale(new Vector3(scaleX, scaleY, 1));
        }

        private static void SetShadowPose(DieView die, float x, float scaleX, float scaleY, float opacity)
        {
            die.Shadow.style.translate = new Translate(x, 0, 0);
            die.Shadow.style.scale = new Scale(new Vector3(scaleX, scaleY, 1));
            die.Shadow.style.opacity = opacity;
        }

        private static void HideGhosts(DieView die)
        {
            foreach (Image ghost in die.Ghosts) ghost.style.opacity = 0;
        }

        private static void ResetDiePose(DieView die)
        {
            if (die.Model3D != null)
            {
                die.Body.style.opacity = 0;
                die.Shadow.style.opacity = 0;
                HideGhosts(die);
                SetModelRest(die, die.Value);
                return;
            }
            SetBodyPose(die, 0, 0, 0, 1, 1);
            SetShadowPose(die, 0, 1, 1, .42f);
            HideGhosts(die);
        }

        private static float Smooth01(float value)
        {
            value = Mathf.Clamp01(value);
            return value * value * (3f - 2f * value);
        }

        private static float EaseOutCubic(float value)
        {
            value = Mathf.Clamp01(value);
            return 1f - Mathf.Pow(1f - value, 3f);
        }

        private void SetDieResult(DieView die, int value, int variant)
        {
            die.Value = value;
            int index = Mathf.Clamp((value - 1) * 2 + variant, 0, Mathf.Max(0, _resultSprites.Length - 1));
            if (_resultSprites.Length > 0) die.Art.sprite = _resultSprites[index];
            die.Art.tooltip = value + " 点";
        }

        private void EmitLandingBurst(DieView die, Color color)
        {
            VisualElement ring = AddElement(die.Root, "LandingRing");
            SetRect(ring, 35, 111, 80, 26);
            Round(ring, 50);
            ring.style.borderLeftWidth = ring.style.borderRightWidth = 4;
            ring.style.borderTopWidth = ring.style.borderBottomWidth = 4;
            ring.style.borderLeftColor = ring.style.borderRightColor = color;
            ring.style.borderTopColor = ring.style.borderBottomColor = color;
            StartCoroutine(AnimateLandingRing(ring));

            for (int i = 0; i < 8; i++)
            {
                VisualElement spark = AddElement(die.Root, "LandingSpark");
                float size = 7 + (i % 4) * 3;
                SetRect(spark, 68, 106, size, size);
                Round(spark, size);
                spark.style.backgroundColor = new Color(color.r, color.g, color.b, .9f);
                StartCoroutine(AnimateSpark(spark, i));
            }
        }

        private IEnumerator AnimateLandingRing(VisualElement ring)
        {
            float elapsed = 0;
            while (elapsed < .32f)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / .32f);
                float scale = Mathf.Lerp(.25f, 1.9f, EaseOutCubic(progress));
                ring.style.scale = new Scale(new Vector3(scale, Mathf.Lerp(.45f, 1f, progress), 1));
                ring.style.opacity = 1f - progress;
                yield return null;
            }
            ring.RemoveFromHierarchy();
        }

        private IEnumerator AnimateSpark(VisualElement spark, int index)
        {
            float elapsed = 0;
            float angle = Mathf.Lerp(-168f, -12f, index / 7f) * Mathf.Deg2Rad;
            while (elapsed < .35f)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / .35f);
                float distance = Mathf.Lerp(0, 92, EaseOutCubic(progress));
                float lift = Mathf.Sin(progress * Mathf.PI) * 18f;
                spark.style.translate = new Translate(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance - lift, 0);
                spark.style.opacity = 1f - progress;
                spark.style.scale = new Scale(new Vector3(1f - progress * .55f, 1f - progress * .55f, 1));
                yield return null;
            }
            spark.RemoveFromHierarchy();
        }

        private IEnumerator PopScore(Label label, int score)
        {
            label.text = score.ToString();
            label.style.color = score >= 18 && score <= 21 ? Gold : label == _playerScoreLabel ? Blue : Red;
            float elapsed = 0;
            while (elapsed < .28f)
            {
                elapsed += Time.deltaTime;
                float p = Mathf.Clamp01(elapsed / .28f);
                float scale = 1f + Mathf.Sin(p * Mathf.PI) * .26f;
                label.style.scale = new Scale(new Vector3(scale, scale, 1));
                yield return null;
            }
            label.style.scale = new Scale(Vector3.one);
        }

        private IEnumerator ShakeArena(float duration)
        {
            VisualElement arena = _document.rootVisualElement.Q("ArenaFrame");
            float elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float amount = (1f - elapsed / duration) * 18f;
                arena.style.translate = new Translate(UnityEngine.Random.Range(-amount, amount), 0, 0);
                yield return null;
            }
            arena.style.translate = new Translate(0, 0, 0);
        }

        private void SetMessage(string kicker, string message, bool robot)
        {
            _messageKicker.text = kicker;
            _messageKicker.style.color = robot ? Red : Hex("#A63D4D");
            _messageLabel.text = message;
        }

        private void AddHistory(string owner, int[] values, Color accent)
        {
            if (_historyEmptyLabel != null) _historyEmptyLabel.style.display = DisplayStyle.None;
            Label item = AddLabel(_historyRow, owner + "  " + values[0] + "+" + values[1], 20, Cream, FontStyle.Bold);
            item.style.width = 126;
            item.style.height = 42;
            item.style.marginLeft = 6;
            item.style.marginRight = 6;
            item.style.backgroundColor = new Color(.02f, .04f, .06f, .58f);
            item.style.borderBottomWidth = 4;
            item.style.borderBottomColor = accent;
            Round(item, 11);
            _historyItems.Add(item);
            if (_historyItems.Count > 5)
            {
                _historyItems[0].RemoveFromHierarchy();
                _historyItems.RemoveAt(0);
            }
        }

        private void EndGame(bool playerWon, string description)
        {
            _gameOver = true;
            _busy = false;
            UpdateButtons();
            _robotGlow.style.opacity = .15f;
            _playerGlow.style.opacity = .15f;
            _statusLabel.text = "●  本局结束";
            _statusLabel.style.color = Muted;

            _resultBadge.text = playerWon ? "胜" : "负";
            _resultBadge.style.backgroundColor = playerWon ? Gold : Hex("#B84861");
            _resultBadge.style.color = playerWon ? Hex("#5C3610") : Cream;
            _resultKicker.text = playerWon ? "LUCKY!" : "SO CLOSE";
            _resultTitle.text = playerWon ? "你赢了！" : "机器人获胜";
            _resultDescription.text = description;
            _resultPlayerScore.text = _playerScore.ToString();
            _resultRobotScore.text = _robotScore.ToString();
            _resultOverlay.style.display = DisplayStyle.Flex;
            _resultCard.style.scale = new Scale(new Vector3(.86f, .86f, 1));
            StartCoroutine(ShowResultCard());
            Play(playerWon ? _winClip : _loseClip);
        }

        private IEnumerator ShowResultCard()
        {
            float elapsed = 0;
            while (elapsed < .35f)
            {
                elapsed += Time.deltaTime;
                float p = Mathf.Clamp01(elapsed / .35f);
                float eased = 1f - Mathf.Pow(1f - p, 3f);
                float scale = Mathf.Lerp(.86f, 1.04f, eased);
                if (p > .78f) scale = Mathf.Lerp(1.04f, 1f, (p - .78f) / .22f);
                _resultCard.style.scale = new Scale(new Vector3(scale, scale, 1));
                yield return null;
            }
            _resultCard.style.scale = new Scale(Vector3.one);
        }

        private void ResetGame()
        {
            StopAllCoroutines();
            _playerScore = 0;
            _robotScore = 0;
            _playerHasRolled = false;
            _busy = false;
            _gameOver = false;
            _playerScoreLabel.text = "0";
            _robotScoreLabel.text = "0";
            _playerScoreLabel.style.color = Blue;
            _robotScoreLabel.style.color = Red;
            _resultOverlay.style.display = DisplayStyle.None;
            _robotGlow.style.opacity = .2f;
            _playerGlow.style.opacity = 1f;
            _statusLabel.text = "●  你的回合";
            _statusLabel.style.color = Blue;
            _tipLabel.text = "你可以随时保存当前点数，但超过 21 会立即落败";
            SetMessage("", "投骰子，别超过 21", false);
            foreach (Label item in _historyItems) item.RemoveFromHierarchy();
            _historyItems.Clear();
            if (_historyEmptyLabel != null) _historyEmptyLabel.style.display = DisplayStyle.Flex;
            SetDieResult(_playerDice[0], 3, 0);
            SetDieResult(_playerDice[1], 5, 0);
            SetDieResult(_robotDice[0], 2, 0);
            SetDieResult(_robotDice[1], 4, 0);
            ResetDiePose(_playerDice[0]);
            ResetDiePose(_playerDice[1]);
            ResetDiePose(_robotDice[0]);
            ResetDiePose(_robotDice[1]);
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool canAct = !_busy && !_gameOver;
            _rollButton.SetEnabled(canAct);
            _holdButton.SetEnabled(canAct && _playerHasRolled);
            _rollButton.style.opacity = canAct ? 1f : .42f;
            _holdButton.style.opacity = canAct && _playerHasRolled ? 1f : .42f;
        }

        private Label AddLabel(VisualElement parent, string text, float size, Color color, FontStyle fontStyle)
        {
            Label label = new Label(text);
            label.style.position = Position.Absolute;
            label.style.fontSize = size;
            label.style.color = color;
            label.style.unityFont = _font;
            label.style.unityFontStyleAndWeight = fontStyle;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            parent.Add(label);
            return label;
        }

        private static VisualElement AddElement(VisualElement parent, string name)
        {
            VisualElement element = new VisualElement { name = name };
            element.style.position = Position.Absolute;
            parent.Add(element);
            return element;
        }

        private static void SetRect(VisualElement element, float left, float top, float width, float height)
        {
            element.style.position = Position.Absolute;
            element.style.left = left;
            element.style.top = top;
            element.style.width = width;
            element.style.height = height;
        }

        private static void Stretch(VisualElement element)
        {
            element.style.position = Position.Absolute;
            element.style.left = 0;
            element.style.right = 0;
            element.style.top = 0;
            element.style.bottom = 0;
        }

        private static void Round(VisualElement element, float radius)
        {
            element.style.borderTopLeftRadius = radius;
            element.style.borderTopRightRadius = radius;
            element.style.borderBottomLeftRadius = radius;
            element.style.borderBottomRightRadius = radius;
        }

        private static void SetBorder(VisualElement element, float width, Color color)
        {
            element.style.borderLeftWidth = width;
            element.style.borderRightWidth = width;
            element.style.borderTopWidth = width;
            element.style.borderBottomWidth = width;
            element.style.borderLeftColor = color;
            element.style.borderRightColor = color;
            element.style.borderTopColor = color;
            element.style.borderBottomColor = color;
        }

        private static Color Hex(string hex)
        {
            Color color;
            return ColorUtility.TryParseHtmlString(hex, out color) ? color : Color.white;
        }
    }
}
