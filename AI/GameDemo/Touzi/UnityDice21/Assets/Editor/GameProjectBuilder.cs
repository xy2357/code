using Dice21;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Dice21Editor
{
    public static class GameProjectBuilder
    {
        [MenuItem("Dice 21/Build Main Scene")]
        public static void Build()
        {
            EnsureFolder("Assets/Scenes");
            EnsureFolder("Assets/Settings");

            const string panelPath = "Assets/Resources/Dice21PanelSettings.asset";
            PanelSettings panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(panelPath);
            if (panelSettings == null)
            {
                panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
                panelSettings.name = "Dice21PanelSettings";
                panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
                panelSettings.referenceResolution = new Vector2Int(1080, 1920);
                panelSettings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
                panelSettings.match = .5f;
                panelSettings.sortingOrder = 0;
                AssetDatabase.CreateAsset(panelSettings, panelPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(panelPath, ImportAssetOptions.ForceSynchronousImport);
                panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(panelPath);
            }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "Main";

            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.05f, .055f, .1f);
            camera.orthographic = true;
            cameraObject.tag = "MainCamera";

            GameObject gameObject = new GameObject("Dice21Game");
            UIDocument document = gameObject.AddComponent<UIDocument>();
            document.panelSettings = panelSettings;
            SerializedObject serializedDocument = new SerializedObject(document);
            SerializedProperty panelProperty = serializedDocument.FindProperty("m_PanelSettings");
            if (panelProperty != null)
            {
                panelProperty.objectReferenceValue = panelSettings;
                serializedDocument.ApplyModifiedPropertiesWithoutUndo();
            }
            EditorUtility.SetDirty(document);
            gameObject.AddComponent<Dice21Game>();

            string scenePath = "Assets/Scenes/Main.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };

            PlayerSettings.productName = "二十一点 · 骰子对决";
            PlayerSettings.companyName = "Touzi Games";
            PlayerSettings.defaultScreenWidth = 540;
            PlayerSettings.defaultScreenHeight = 960;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.runInBackground = true;
            PlayerSettings.colorSpace = ColorSpace.Linear;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(gameObject);
            Selection.activeGameObject = gameObject;
            Debug.Log("[Dice21] Main scene generated successfully.");
        }

        [MenuItem("Dice 21/Build Windows Player")]
        public static void BuildWindows()
        {
            string scenePath = "Assets/Scenes/Main.unity";
            Build();

            string buildDirectory = System.IO.Path.GetFullPath("Build");
            System.IO.Directory.CreateDirectory(buildDirectory);
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { scenePath },
                locationPathName = System.IO.Path.Combine(buildDirectory, "Dice21.exe"),
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new System.Exception("Windows build failed: " + report.summary.result);
            }
            Debug.Log("[Dice21] Windows build completed: " + options.locationPathName);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string[] parts = path.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
