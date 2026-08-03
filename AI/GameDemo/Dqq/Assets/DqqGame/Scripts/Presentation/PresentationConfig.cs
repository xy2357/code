using System;
using System.Collections.Generic;
using UnityEngine;

namespace DqqGame.Presentation
{
    [Serializable]
    public sealed class AbilityPresentationConfig
    {
        public int abilityId;
        public string accent;
        public string castLabel;
        public int hitTimeMs;
        public int totalTimeMs;
        public bool faceTarget;
        public string hitReaction;
        public string floatingTextType;
    }

    [Serializable]
    public sealed class PresentationConfigList
    {
        public AbilityPresentationConfig[] abilities;
    }

    public static class PresentationCatalog
    {
        private static Dictionary<int, AbilityPresentationConfig> configs;
        private static readonly AbilityPresentationConfig Default = new AbilityPresentationConfig
        {
            abilityId = 0,
            accent = "#F5F7FF",
            castLabel = "技能触发",
            hitTimeMs = 160,
            totalTimeMs = 360,
            faceTarget = true,
            hitReaction = "HitLight",
            floatingTextType = "PhysicalDamage"
        };

        public static AbilityPresentationConfig Get(int abilityId)
        {
            if (configs == null) Load();
            return configs.TryGetValue(abilityId, out AbilityPresentationConfig config) ? config : Default;
        }

        private static void Load()
        {
            configs = new Dictionary<int, AbilityPresentationConfig>();
            TextAsset json = Resources.Load<TextAsset>("Config/presentation");
            if (json == null) return;
            PresentationConfigList list = JsonUtility.FromJson<PresentationConfigList>(json.text);
            foreach (AbilityPresentationConfig config in list.abilities)
                configs[config.abilityId] = config;
        }
    }
}
