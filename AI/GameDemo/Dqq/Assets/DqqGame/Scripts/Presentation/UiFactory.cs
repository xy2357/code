using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DqqGame.Presentation
{
    public static class UiFactory
    {
        private static Font font;
        private static Sprite roundedSprite;
        private static Sprite circleSprite;
        private static Sprite softCircleSprite;
        private static Sprite adventurePanelSprite;
        private static Sprite adventureButtonSprite;
        private static Sprite adventureHexSprite;
        private static Sprite adventureGridSprite;

        public static readonly Color Background = Hex("#080B18");
        public static readonly Color Panel = Hex("#11172B");
        public static readonly Color PanelLight = Hex("#18213B");
        public static readonly Color Cyan = Hex("#5AE8FF");
        public static readonly Color Pink = Hex("#FF4F9A");
        public static readonly Color Lime = Hex("#C8FF4A");
        public static readonly Color Muted = Hex("#8D9AB8");
        public static readonly Color White = Hex("#F5F7FF");

        public static Font Font
        {
            get
            {
                if (font == null)
                {
                    // 使用系统中文字体，不把任何商业字体文件打进工程。
                    font = Font.CreateDynamicFontFromOSFont(
                        new[] { "Microsoft YaHei UI", "Microsoft YaHei", "SimHei", "Arial" }, 32);
                    if (font == null) font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                }
                return font;
            }
        }

        public static Sprite RoundedSprite => roundedSprite != null ? roundedSprite : roundedSprite = MakeRoundedRect();
        public static Sprite CircleSprite => circleSprite != null ? circleSprite : circleSprite = MakeCircle(false);
        public static Sprite SoftCircleSprite => softCircleSprite != null ? softCircleSprite : softCircleSprite = MakeCircle(true);
        public static Sprite AdventurePanelSprite => adventurePanelSprite != null ? adventurePanelSprite :
            adventurePanelSprite = Resources.Load<Sprite>("Art/UI/Adventure/panel_grey_bolts_detail_a") ?? RoundedSprite;
        public static Sprite AdventureButtonSprite => adventureButtonSprite != null ? adventureButtonSprite :
            adventureButtonSprite = Resources.Load<Sprite>("Art/UI/Adventure/button_grey") ?? RoundedSprite;
        public static Sprite AdventureHexSprite => adventureHexSprite != null ? adventureHexSprite :
            adventureHexSprite = Resources.Load<Sprite>("Art/UI/Adventure/hexagon_grey") ?? CircleSprite;
        public static Sprite AdventureGridSprite => adventureGridSprite != null ? adventureGridSprite :
            adventureGridSprite = Resources.Load<Sprite>("Art/UI/Adventure/panel_grid_blueprint") ?? RoundedSprite;

        public static RectTransform Rect(string name, Transform parent, Color color,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax,
            Sprite sprite = null)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            Image image = go.GetComponent<Image>();
            bool adventurePanel = sprite == null;
            image.color = adventurePanel
                ? Color.Lerp(new Color(.46f, .52f, .62f, color.a), color, .72f)
                : color;
            image.sprite = adventurePanel ? AdventurePanelSprite : sprite;
            image.type = Image.Type.Sliced;
            return rect;
        }

        public static Text Text(string name, Transform parent, string value, int size, Color color,
            TextAnchor alignment, FontStyle style = FontStyle.Normal)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            go.transform.SetParent(parent, false);
            Text text = go.GetComponent<Text>();
            text.font = Font;
            text.text = value;
            text.fontSize = size;
            text.color = color;
            text.alignment = alignment;
            text.fontStyle = style;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            // Text is presentational only. Full-screen Text objects must not block buttons below them.
            text.raycastTarget = false;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return text;
        }

        public static Button Button(string name, Transform parent, string label, Color color, UnityAction onClick)
        {
            RectTransform rect = Rect(name, parent, color, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero,
                AdventureButtonSprite);
            Button button = rect.gameObject.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1.1f, 1.1f, 1.1f, 1f);
            colors.pressedColor = new Color(.78f, .78f, .78f, 1f);
            colors.selectedColor = Color.white;
            colors.fadeDuration = .08f;
            button.colors = colors;
            button.targetGraphic = rect.GetComponent<Image>();
            button.onClick.AddListener(onClick);
            Text text = Text("Label", rect, label, 26, Background, TextAnchor.MiddleCenter, FontStyle.Bold);
            text.raycastTarget = false;
            return button;
        }

        public static Canvas CreateCanvas()
        {
            GameObject canvasGo = new GameObject("DQQ Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = .5f;

            if (Object.FindFirstObjectByType<EventSystem>() == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            return canvas;
        }

        public static Color Hex(string value)
        {
            return ColorUtility.TryParseHtmlString(value, out Color parsed) ? parsed : Color.white;
        }

        public static void AddOutline(Graphic graphic, Color color, Vector2 distance)
        {
            Outline outline = graphic.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = distance;
            outline.useGraphicAlpha = true;
        }

        private static Sprite MakeRoundedRect()
        {
            const int size = 64;
            const float radius = 15f;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = "DQQ Rounded Rect";
            texture.wrapMode = TextureWrapMode.Clamp;
            Color32[] pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = Mathf.Max(radius - x, 0f, x - (size - 1 - radius));
                float dy = Mathf.Max(radius - y, 0f, y - (size - 1 - radius));
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                byte alpha = (byte)(Mathf.Clamp01(radius + .75f - distance) * 255);
                pixels[y * size + x] = new Color32(255, 255, 255, alpha);
            }
            texture.SetPixels32(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.one * .5f, 100f,
                0, SpriteMeshType.FullRect, new Vector4(16, 16, 16, 16));
        }

        private static Sprite MakeCircle(bool soft)
        {
            const int size = 128;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = soft ? "DQQ Soft Circle" : "DQQ Circle";
            texture.wrapMode = TextureWrapMode.Clamp;
            Color32[] pixels = new Color32[size * size];
            Vector2 center = Vector2.one * (size - 1) * .5f;
            float radius = size * .48f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float normalized = Vector2.Distance(new Vector2(x, y), center) / radius;
                float alpha = soft ? Mathf.Clamp01(1f - normalized) : Mathf.Clamp01((1f - normalized) * 12f);
                if (soft) alpha *= alpha;
                pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
            }
            texture.SetPixels32(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.one * .5f, 100f);
        }
    }
}
