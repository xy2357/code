#if UNITY_EDITOR
using System.IO;
using System.Linq;
using DqqGame.Combat;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DqqGame.Editor
{
    public static class ProjectSetup
    {
        private const string ScenePath = "Assets/DqqGame/Scenes/Main.unity";

        [MenuItem("DQQ/Configure Project")]
        public static void Configure()
        {
            ExcelConfigImporter.Import(false);
            ConfigureMonsterSprites();
            ConfigureUiSprites();
            ConfigureHeroModels();
            Directory.CreateDirectory("Assets/DqqGame/Scenes");
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject cameraGo = new GameObject("Main Camera", typeof(Camera));
            Camera camera = cameraGo.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.03f, .04f, .08f);
            camera.orthographic = true;
            cameraGo.tag = "MainCamera";
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };

            PlayerSettings.productName = "电子斗蛐蛐 - 蛐蛐协议";
            PlayerSettings.companyName = "DQQ Lab";
            PlayerSettings.defaultScreenWidth = 1600;
            PlayerSettings.defaultScreenHeight = 900;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.runInBackground = true;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, "com.dqqlab.autobattler");
            AssetDatabase.SaveAssets();
            Debug.Log("DQQ project configured successfully.");
        }

        private static void ConfigureMonsterSprites()
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/DqqGame/Resources/Art/Monsters" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null) continue;
                bool changed = importer.textureType != TextureImporterType.Sprite ||
                               importer.spriteImportMode != SpriteImportMode.Single;
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Compressed;
                if (changed) importer.SaveAndReimport();
            }
        }

        private static void ConfigureUiSprites()
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/DqqGame/Resources/Art/UI/Adventure" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null) continue;
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                string name = Path.GetFileNameWithoutExtension(path);
                if (name.StartsWith("panel_")) importer.spriteBorder = new Vector4(24, 24, 24, 24);
                else if (name.StartsWith("button_")) importer.spriteBorder = new Vector4(20, 12, 20, 12);
                else importer.spriteBorder = Vector4.zero;
                importer.SaveAndReimport();
            }
        }

        private static void ConfigureHeroModels()
        {
            string[] guids = AssetDatabase.FindAssets("t:Model", new[] { "Assets/DqqGame/Resources/Art/Heroes" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
                if (importer == null) continue;
                importer.importAnimation = true;
                importer.animationType = ModelImporterAnimationType.Generic;
                importer.SaveAndReimport();
            }
        }

        [MenuItem("DQQ/Verify Combat Framework")]
        public static void VerifyCombatFramework()
        {
            GameConfig.EnsureLoaded();
            BuildState build = new BuildState();
            build.AbilityIds.Add(100101);
            build.AbilityIds.Add(100301);
            build.AbilityIds.Add(100401);
            build.AbilityIds.Add(100601);
            CombatWorld world = new CombatWorld(build, 4, 424242);
            BattleResult result = world.Run();

            bool hasAttack = false;
            bool hasDamage = false;
            bool hasAbility = false;
            bool ordered = true;
            int tick = -1;
            int sequence = -1;
            foreach (BattleViewEvent item in result.Events)
            {
                hasAttack |= item.Type == BattleViewEventType.AttackStarted;
                hasDamage |= item.Type == BattleViewEventType.DamageResolved;
                hasAbility |= item.Type == BattleViewEventType.AbilityStarted;
                if (item.Tick < tick || (item.Tick == tick && item.Sequence <= sequence)) ordered = false;
                tick = item.Tick;
                sequence = item.Sequence;
            }

            if (!hasAttack || !hasDamage || !hasAbility || !ordered)
                throw new System.Exception("Combat framework verification failed.");
            Debug.Log($"DQQ_VERIFY_OK events={result.Events.Count} won={result.PlayerWon} duration={result.DurationMs}");
        }

        [MenuItem("DQQ/Run Six-School Balance Smoke Test")]
        public static void RunBalanceSmokeTest()
        {
            GameConfig.EnsureLoaded();
            int[] wins = new int[7];
            int[] games = new int[7];
            for (int leftHero = 1; leftHero <= 6; leftHero++)
            for (int rightHero = leftHero + 1; rightHero <= 6; rightHero++)
            for (int seed = 0; seed < 16; seed++)
            {
                BuildState left = SchoolBuild(leftHero);
                BuildState right = SchoolBuild(rightHero);
                CombatWorld world = new CombatWorld(left, right, 4,
                    900000 + leftHero * 10000 + rightHero * 100 + seed);
                BattleResult result = world.Run();
                games[leftHero]++;
                games[rightHero]++;
                if (result.PlayerWon) wins[leftHero]++;
                else wins[rightHero]++;
                if (!result.Events.Any(item => item.Type == BattleViewEventType.AbilityStarted))
                    throw new System.Exception("A balance match produced no ability events.");
            }

            string report = string.Join(" | ", Enumerable.Range(1, 6).Select(id =>
                $"H{id}:{wins[id]}/{games[id]}={wins[id] * 100f / games[id]:0.0}%"));
            Debug.Log("DQQ_BALANCE_OK " + report);
        }

        private static BuildState SchoolBuild(int heroId)
        {
            HeroConfig hero = GameConfig.Hero(heroId);
            BuildState build = new BuildState { HeroId = heroId };
            foreach (UpgradeConfig upgrade in GameConfig.Upgrades.Where(item => item.school == hero.school).Take(4))
                build.Apply(upgrade);
            return build;
        }

        [MenuItem("DQQ/Build Windows")]
        public static void BuildWindows()
        {
            Configure();
            Directory.CreateDirectory("Builds/Windows");
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = "Builds/Windows/Dqq.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new System.Exception($"Windows build failed: {report.summary.result}");
            Debug.Log($"DQQ_BUILD_OK size={report.summary.totalSize}");
        }
    }
}
#endif
