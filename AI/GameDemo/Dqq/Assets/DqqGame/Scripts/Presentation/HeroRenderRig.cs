using System;
using System.Linq;
using DqqGame.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace DqqGame.Presentation
{
    public sealed class HeroRenderRig : MonoBehaviour
    {
        private static int nextWorldSlot;
        private const int RenderLayer = 29;

        private GameObject sceneRoot;
        private Transform modelPivot;
        private GameObject model;
        private Camera renderCamera;
        private RenderTexture renderTexture;
        private RawImage viewport;
        private AnimationClip idleClip;
        private AnimationClip activeClip;
        private float clipTime;
        private float activeUntil;
        private Vector3 worldOrigin;
        private Vector3 pivotHome;
        private bool playerSide;
        private string resourceName;

        public RawImage Build(RectTransform parent, bool isPlayerSide, int textureSize)
        {
            playerSide = isPlayerSide;
            worldOrigin = new Vector3((nextWorldSlot++ % 32) * 35f, -800f, (nextWorldSlot / 32) * 35f);

            GameObject viewportObject = new GameObject("3D Hero Viewport", typeof(RectTransform),
                typeof(CanvasRenderer), typeof(RawImage));
            viewportObject.transform.SetParent(parent, false);
            RectTransform rect = viewportObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            viewport = viewportObject.GetComponent<RawImage>();
            viewport.color = Color.white;
            viewport.raycastTarget = false;

            renderTexture = new RenderTexture(textureSize, textureSize, 24, RenderTextureFormat.ARGB32)
            {
                name = "DQQ Hero Portrait",
                antiAliasing = textureSize >= 512 ? 4 : 2,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                useMipMap = false,
            };
            renderTexture.Create();
            viewport.texture = renderTexture;

            sceneRoot = new GameObject("DQQ Hero Render Scene");
            sceneRoot.transform.position = worldOrigin;
            modelPivot = new GameObject("Model Pivot").transform;
            modelPivot.SetParent(sceneRoot.transform, false);
            pivotHome = Vector3.zero;

            GameObject cameraObject = new GameObject("Portrait Camera", typeof(Camera));
            cameraObject.transform.SetParent(sceneRoot.transform, false);
            cameraObject.transform.localPosition = new Vector3(0, 1.25f, -4.5f);
            cameraObject.transform.LookAt(worldOrigin + new Vector3(0, 1.18f, 0));
            renderCamera = cameraObject.GetComponent<Camera>();
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = new Color(0, 0, 0, 0);
            renderCamera.orthographic = true;
            renderCamera.orthographicSize = 1.48f;
            renderCamera.nearClipPlane = .1f;
            renderCamera.farClipPlane = 12f;
            renderCamera.cullingMask = 1 << RenderLayer;
            renderCamera.targetTexture = renderTexture;
            renderCamera.allowHDR = true;

            CreateLight("Key Light", new Vector3(35, -35, 0), new Color(1f, .84f, .67f), 1.25f);
            CreateLight("Rim Light", new Vector3(330, 145, 0), new Color(.35f, .65f, 1f), .9f);
            return viewport;
        }

        public void SetHero(HeroConfig hero)
        {
            if (hero == null) return;
            string resource = string.IsNullOrWhiteSpace(hero.modelResource) ? "Viking_Male" : hero.modelResource;
            resourceName = resource;
            GameObject prefab = Resources.Load<GameObject>("Art/Heroes/" + resource);
            if (prefab == null)
            {
                Debug.LogWarning("Missing 3D hero resource: " + resource);
                return;
            }

            if (model != null) Destroy(model);
            model = Instantiate(prefab, modelPivot, false);
            model.name = hero.heroName + " · " + resource;
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            model.transform.localScale = Vector3.one;
            SetLayerRecursively(model, RenderLayer);
            Animator animator = model.GetComponent<Animator>();
            if (animator == null) animator = model.AddComponent<Animator>();
            animator.applyRootMotion = false;
            foreach (SkinnedMeshRenderer skin in model.GetComponentsInChildren<SkinnedMeshRenderer>())
                skin.updateWhenOffscreen = true;
            FrameModel();

            AnimationClip[] clips = Resources.LoadAll<AnimationClip>("Art/Heroes/" + resource)
                .Where(clip => !clip.name.StartsWith("__preview__", StringComparison.OrdinalIgnoreCase)).ToArray();
            idleClip = FindClip(clips, "Idle") ?? clips.FirstOrDefault();
            activeClip = idleClip;
            clipTime = 0;
        }

        public void PlayAttack() => PlayNamed(.52f, "Attack", "Punch", "Slash");
        public void PlayCast() => PlayNamed(.72f, "Spell", "Cast", "Magic");
        public void PlayHit() => PlayNamed(.38f, "Hit", "Damage", "Impact");
        public void PlayDodge() => PlayNamed(.48f, "Dodge", "Roll", "Jump");
        public void PlayDeath() => PlayNamed(1.2f, "Death", "Die");

        private void Update()
        {
            if (model == null) return;
            AnimationClip clip = Time.unscaledTime < activeUntil && activeClip != null ? activeClip : idleClip;
            if (clip != null && clip.length > 0)
            {
                clipTime += Time.unscaledDeltaTime;
                clip.SampleAnimation(model, clipTime % clip.length);
            }
            float bob = Mathf.Sin(Time.unscaledTime * 2.1f + worldOrigin.x) * .018f;
            modelPivot.localPosition = pivotHome + Vector3.up * bob;
        }

        private void PlayNamed(float duration, params string[] hints)
        {
            if (model == null) return;
            AnimationClip[] clips = Resources.LoadAll<AnimationClip>("Art/Heroes/" + resourceName);
            activeClip = hints.Select(hint => FindClip(clips, hint)).FirstOrDefault(clip => clip != null) ?? idleClip;
            clipTime = 0;
            activeUntil = Time.unscaledTime + duration;
        }

        private void FrameModel()
        {
            Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return;
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            float scale = 2.45f / Mathf.Max(.1f, bounds.size.y);
            model.transform.localScale = Vector3.one * scale;

            bounds = model.GetComponentsInChildren<Renderer>()[0].bounds;
            foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>()) bounds.Encapsulate(renderer.bounds);
            Vector3 target = worldOrigin + new Vector3(0, 1.18f, 0);
            model.transform.position += target - bounds.center;
        }

        private void CreateLight(string lightName, Vector3 euler, Color color, float intensity)
        {
            GameObject lightObject = new GameObject(lightName, typeof(Light));
            lightObject.transform.SetParent(sceneRoot.transform, false);
            lightObject.transform.localRotation = Quaternion.Euler(euler);
            Light light = lightObject.GetComponent<Light>();
            light.type = LightType.Directional;
            light.color = color;
            light.intensity = intensity;
            light.cullingMask = 1 << RenderLayer;
        }

        private static AnimationClip FindClip(AnimationClip[] clips, string hint)
        {
            return clips.FirstOrDefault(clip => clip.name.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static void SetLayerRecursively(GameObject target, int layer)
        {
            target.layer = layer;
            foreach (Transform child in target.transform) SetLayerRecursively(child.gameObject, layer);
        }

        private void OnDestroy()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }
            if (sceneRoot != null) Destroy(sceneRoot);
        }
    }
}
