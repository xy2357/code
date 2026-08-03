using System.Collections;
using System.Collections.Generic;
using DqqGame.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace DqqGame.Presentation
{
    public sealed class FighterView : MonoBehaviour
    {
        private RectTransform root;
        private RectTransform body;
        private RectTransform shadow;
        private RawImage heroImage;
        private HeroRenderRig heroRig;
        private Image coreImage;
        private Image healthFill;
        private Image energyFill;
        private Text healthText;
        private Text nameText;
        private Text statusText;
        private Color accent;
        private Vector2 home;
        private bool isPlayer;
        private Coroutine motion;
        private readonly List<Image> artImages = new List<Image>();

        public RectTransform Root => root;

        public void Build(Transform parent, bool playerSide)
        {
            isPlayer = playerSide;
            accent = playerSide ? UiFactory.Cyan : UiFactory.Pink;

            root = UiFactory.Rect(playerSide ? "Player Fighter" : "Enemy Fighter", parent, Color.clear,
                new Vector2(.5f, .5f), new Vector2(.5f, .5f), Vector2.zero, Vector2.zero);
            root.sizeDelta = new Vector2(420, 520);
            home = playerSide ? new Vector2(-430, -20) : new Vector2(430, -20);
            root.anchoredPosition = home;

            shadow = UiFactory.Rect("Shadow", root, new Color(0, 0, 0, .5f),
                new Vector2(.5f, .5f), new Vector2(.5f, .5f), Vector2.zero, Vector2.zero, UiFactory.SoftCircleSprite);
            shadow.sizeDelta = new Vector2(300, 86);
            shadow.anchoredPosition = new Vector2(0, -176);

            RectTransform glow = UiFactory.Rect("Glow", root, new Color(accent.r, accent.g, accent.b, .24f),
                new Vector2(.5f, .5f), new Vector2(.5f, .5f), Vector2.zero, Vector2.zero, UiFactory.SoftCircleSprite);
            glow.sizeDelta = new Vector2(390, 390);
            glow.anchoredPosition = new Vector2(0, -25);
            coreImage = glow.GetComponent<Image>();

            GameObject artGo = new GameObject("Quaternius 3D Hero", typeof(RectTransform));
            artGo.transform.SetParent(root, false);
            body = artGo.GetComponent<RectTransform>();
            body.anchorMin = body.anchorMax = new Vector2(.5f, .5f);
            body.sizeDelta = new Vector2(330, 390);
            body.anchoredPosition = new Vector2(0, -20);

            heroRig = gameObject.AddComponent<HeroRenderRig>();
            heroImage = heroRig.Build(body, playerSide, 512);

            RectTransform plate = UiFactory.Rect("Name Plate", root, UiFactory.PanelLight,
                new Vector2(.5f, 1f), new Vector2(.5f, 1f), Vector2.zero, Vector2.zero);
            plate.sizeDelta = new Vector2(330, 54);
            plate.anchoredPosition = new Vector2(0, -16);
            nameText = UiFactory.Text("Name", plate, "斗士", 25, UiFactory.White, TextAnchor.MiddleCenter, FontStyle.Bold);

            RectTransform hpBack = UiFactory.Rect("HP Back", root, new Color(0.03f, .04f, .09f, .94f),
                new Vector2(.5f, 1f), new Vector2(.5f, 1f), Vector2.zero, Vector2.zero);
            hpBack.sizeDelta = new Vector2(330, 30);
            hpBack.anchoredPosition = new Vector2(0, -80);
            RectTransform hp = UiFactory.Rect("HP Fill", hpBack, playerSide ? UiFactory.Lime : UiFactory.Pink,
                Vector2.zero, Vector2.one, new Vector2(4, 4), new Vector2(-4, -4));
            healthFill = hp.GetComponent<Image>();
            healthFill.type = Image.Type.Filled;
            healthFill.fillMethod = Image.FillMethod.Horizontal;
            healthText = UiFactory.Text("HP Text", hpBack, "100 / 100", 17, UiFactory.White, TextAnchor.MiddleCenter, FontStyle.Bold);

            RectTransform energyBack = UiFactory.Rect("Energy Back", root, new Color(0.03f, .04f, .09f, .94f),
                new Vector2(.5f, 1f), new Vector2(.5f, 1f), Vector2.zero, Vector2.zero);
            energyBack.sizeDelta = new Vector2(270, 13);
            energyBack.anchoredPosition = new Vector2(0, -116);
            RectTransform energy = UiFactory.Rect("Energy Fill", energyBack, UiFactory.Hex("#B996FF"),
                Vector2.zero, Vector2.one, new Vector2(2, 2), new Vector2(-2, -2));
            energyFill = energy.GetComponent<Image>();
            energyFill.type = Image.Type.Filled;
            energyFill.fillMethod = Image.FillMethod.Horizontal;

            statusText = UiFactory.Text("Status", root, string.Empty, 23, accent, TextAnchor.MiddleCenter, FontStyle.Bold);
            RectTransform statusRect = statusText.rectTransform;
            statusRect.anchorMin = statusRect.anchorMax = new Vector2(.5f, .5f);
            statusRect.sizeDelta = new Vector2(360, 48);
            statusRect.anchoredPosition = new Vector2(0, -235);
        }

        public void ResetForBattle(int heroId, string fighterName, int maxHealth)
        {
            gameObject.SetActive(true);
            root.anchoredPosition = home;
            root.localScale = Vector3.one;
            root.localRotation = Quaternion.identity;
            foreach (Image image in artImages) image.color = Color.white;
            if (heroImage != null) heroImage.color = Color.white;
            heroRig.SetHero(GameConfig.Hero(heroId));
            coreImage.color = new Color(accent.r, accent.g, accent.b, .24f);
            nameText.text = fighterName;
            SetHealth(maxHealth, maxHealth);
            SetEnergy(0, 100);
            statusText.text = "准备就绪";
            statusText.color = new Color(accent.r, accent.g, accent.b, .75f);
        }

        public void SetHealth(int current, int maximum)
        {
            healthFill.fillAmount = maximum <= 0 ? 0 : Mathf.Clamp01(current / (float)maximum);
            healthText.text = $"{Mathf.Max(0, current)} / {maximum}";
        }

        public void SetEnergy(int current, int maximum)
        {
            energyFill.fillAmount = maximum <= 0 ? 0 : Mathf.Clamp01(current / (float)maximum);
        }

        public void Attack()
        {
            heroRig.PlayAttack();
            PlayMotion(AttackRoutine());
        }

        public void Dodge()
        {
            heroRig.PlayDodge();
            PlayMotion(DodgeRoutine());
            FlashStatus("闪避!", UiFactory.Lime);
        }

        public void Cast(Color color, string label)
        {
            heroRig.PlayCast();
            PlayMotion(CastRoutine(color));
            FlashStatus(label, color);
        }

        public void Hit(Color color, bool critical)
        {
            heroRig.PlayHit();
            if (motion == null) PlayMotion(HitRoutine());
            foreach (Image image in artImages)
                image.color = Color.Lerp(Color.white, color, .55f);
            if (heroImage != null) heroImage.color = Color.Lerp(Color.white, color, .32f);
            coreImage.color = color;
            Invoke(nameof(RestoreColor), .12f);
            if (critical) FlashStatus("暴击!", UiFactory.Hex("#FFD166"));
        }

        public void Heal()
        {
            coreImage.color = UiFactory.Hex("#58FFB5");
            Invoke(nameof(RestoreColor), .18f);
        }

        public void SetBuff(string label, Color color)
        {
            FlashStatus(label, color);
        }

        public void Die()
        {
            heroRig.PlayDeath();
            PlayMotion(DeathRoutine());
            FlashStatus("失去战斗能力", UiFactory.Muted);
        }

        private Image CreateArtPart(string partName, Transform parent, string resourceName,
            Vector2 position, Vector2 size, bool mirror, float rotation)
        {
            Sprite sprite = Resources.Load<Sprite>("Art/Monsters/" + resourceName);
            GameObject go = new GameObject(partName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            rect.localScale = new Vector3(mirror ? -1 : 1, 1, 1);
            rect.localEulerAngles = new Vector3(0, 0, rotation);
            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;
            image.raycastTarget = false;
            artImages.Add(image);
            return image;
        }

        private void PlayMotion(IEnumerator routine)
        {
            if (motion != null) StopCoroutine(motion);
            motion = StartCoroutine(routine);
        }

        private IEnumerator AttackRoutine()
        {
            Vector2 target = home + new Vector2(isPlayer ? 105 : -105, 8);
            yield return MoveTo(target, .1f);
            yield return MoveTo(home, .17f);
            motion = null;
        }

        private IEnumerator DodgeRoutine()
        {
            Vector2 target = home + new Vector2(isPlayer ? -58 : 58, 74);
            root.localRotation = Quaternion.Euler(0, 0, isPlayer ? 9 : -9);
            yield return MoveTo(target, .1f);
            yield return MoveTo(home, .2f);
            root.localRotation = Quaternion.identity;
            motion = null;
        }

        private IEnumerator CastRoutine(Color color)
        {
            coreImage.color = color;
            for (int i = 0; i < 2; i++)
            {
                yield return ScaleTo(Vector3.one * 1.12f, .08f);
                yield return ScaleTo(Vector3.one, .08f);
            }
            RestoreColor();
            motion = null;
        }

        private IEnumerator HitRoutine()
        {
            Vector2 target = home + new Vector2(isPlayer ? -28 : 28, 0);
            yield return MoveTo(target, .05f);
            yield return MoveTo(home, .12f);
            motion = null;
        }

        private IEnumerator DeathRoutine()
        {
            Vector2 from = root.anchoredPosition;
            Vector2 to = home + new Vector2(isPlayer ? -90 : 90, -90);
            float elapsed = 0;
            while (elapsed < .55f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / .55f);
                root.anchoredPosition = Vector2.Lerp(from, to, t);
                root.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, isPlayer ? 22 : -22, t));
                root.localScale = Vector3.Lerp(Vector3.one, Vector3.one * .82f, t);
                yield return null;
            }
            foreach (Image image in artImages) image.color = UiFactory.Muted;
            if (heroImage != null) heroImage.color = UiFactory.Muted;
            coreImage.color = UiFactory.Muted;
            motion = null;
        }

        private IEnumerator MoveTo(Vector2 target, float duration)
        {
            Vector2 from = root.anchoredPosition;
            float elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0, 1, Mathf.Clamp01(elapsed / duration));
                root.anchoredPosition = Vector2.Lerp(from, target, t);
                yield return null;
            }
            root.anchoredPosition = target;
        }

        private IEnumerator ScaleTo(Vector3 target, float duration)
        {
            Vector3 from = root.localScale;
            float elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                root.localScale = Vector3.Lerp(from, target, Mathf.Clamp01(elapsed / duration));
                yield return null;
            }
            root.localScale = target;
        }

        private void FlashStatus(string value, Color color)
        {
            statusText.text = value;
            statusText.color = color;
            CancelInvoke(nameof(ClearStatus));
            Invoke(nameof(ClearStatus), .75f);
        }

        private void ClearStatus()
        {
            statusText.text = string.Empty;
        }

        private void RestoreColor()
        {
            foreach (Image image in artImages) image.color = Color.white;
            if (heroImage != null) heroImage.color = Color.white;
            coreImage.color = new Color(accent.r, accent.g, accent.b, .24f);
        }
    }
}
