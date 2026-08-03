using System;
using System.Collections.Generic;
using UnityEngine;

namespace DqqGame.Combat
{
    public enum BattleViewEventType
    {
        BattleStarted,
        AttackStarted,
        AbilityStarted,
        DodgeSucceeded,
        DamageResolved,
        HealResolved,
        EnergyChanged,
        BuffAdded,
        BuffRemoved,
        UnitDied,
        BattleEnded
    }

    public enum TriggerEventType
    {
        BattleStart,
        AfterBasicAttack,
        DodgeSucceeded,
        DamageResolved,
        EnergyFull,
        AfterUltimate
    }

    public enum EffectType
    {
        Damage,
        RepeatDamage,
        HealFromEvent,
        AddBurn,
        AddFrost,
        GainEnergy,
        DetonateBurn,
        TemporaryDodge
    }

    public enum TargetRule
    {
        Enemy,
        EventSource,
        Self
    }

    public enum ConditionType
    {
        Always,
        EventTargetIsOwner,
        EventSourceIsOwner,
        EventWasCritical
    }

    [Serializable]
    public sealed class EffectConfig
    {
        public string effectType;
        public int coefficientBP;
        public int flatValue;
        public string element;
        public string buffId;
        public int repeatCount = 1;
        public int durationMs;
        public bool guaranteedCritical;
    }

    [Serializable]
    public sealed class AbilityConfig
    {
        public int abilityId;
        public string abilityName;
        public string description;
        public string triggerEvent;
        public int triggerCount = 1;
        public int triggerChanceBP = 10000;
        public int internalCooldownMs;
        public string condition;
        public string targetRule;
        public string tags;
        public int maxTriggersPerChain = 1;
        public int energyCost;
        public bool isUltimate;
        public EffectConfig[] effects;

        public TriggerEventType Trigger => Parse(triggerEvent, TriggerEventType.AfterBasicAttack);
        public TargetRule Target => Parse(targetRule, TargetRule.Enemy);
        public ConditionType Condition => Parse(condition, ConditionType.Always);

        private static T Parse<T>(string value, T fallback) where T : struct
        {
            return Enum.TryParse(value, true, out T parsed) ? parsed : fallback;
        }
    }

    [Serializable]
    public sealed class AbilityConfigList
    {
        public AbilityConfig[] abilities;
    }

    [Serializable]
    public sealed class UpgradeConfig
    {
        public string upgradeId;
        public string upgradeName;
        public string description;
        public string icon;
        public string accent;
        public int attackBP;
        public int healthBP;
        public int attackSpeedBP;
        public int dodgeBP;
        public int critBP;
        public int defenseFlat;
        public int addAbilityId;
        public bool unique;
        public string school;
        public string rarity;
        public int critDamageBP;
        public int basicPowerBP;
        public int ultimatePowerBP;
        public int energyGainBP;
        public int burnPowerBP;
        public int frostPowerBP;
        public int counterPowerBP;
    }

    [Serializable]
    public sealed class UpgradeConfigList
    {
        public UpgradeConfig[] upgrades;
    }

    [Serializable]
    public sealed class HeroConfig
    {
        public int heroId;
        public string heroName;
        public string title;
        public string school;
        public string accent;
        public string passiveName;
        public string passiveDescription;
        public int passiveAbilityId;
        public string ultimateName;
        public string ultimateDescription;
        public int ultimateAbilityId;
        public int baseHealth;
        public int baseAttack;
        public int baseDefense;
        public int attackIntervalMs;
        public int dodgeBP;
        public int critBP;
        public string modelResource;
    }

    [Serializable]
    public sealed class HeroConfigList
    {
        public HeroConfig[] heroes;
    }

    public static class GameConfig
    {
        private static Dictionary<int, AbilityConfig> abilities;
        private static List<UpgradeConfig> upgrades;
        private static Dictionary<string, UpgradeConfig> upgradesById;
        private static Dictionary<int, HeroConfig> heroes;

        public static IReadOnlyList<UpgradeConfig> Upgrades
        {
            get
            {
                EnsureLoaded();
                return upgrades;
            }
        }

        public static AbilityConfig Ability(int id)
        {
            EnsureLoaded();
            if (!abilities.TryGetValue(id, out AbilityConfig value))
                throw new InvalidOperationException($"Ability config not found: {id}");
            return value;
        }

        public static UpgradeConfig Upgrade(string id)
        {
            EnsureLoaded();
            return id != null && upgradesById.TryGetValue(id, out UpgradeConfig value) ? value : null;
        }

        public static IReadOnlyCollection<HeroConfig> Heroes
        {
            get
            {
                EnsureLoaded();
                return heroes.Values;
            }
        }

        public static HeroConfig Hero(int id)
        {
            EnsureLoaded();
            if (!heroes.TryGetValue(id, out HeroConfig value))
                throw new InvalidOperationException($"Hero config not found: {id}");
            return value;
        }

        public static void EnsureLoaded()
        {
            if (abilities != null) return;

            TextAsset abilityJson = Resources.Load<TextAsset>("Config/abilities");
            TextAsset upgradeJson = Resources.Load<TextAsset>("Config/upgrades");
            TextAsset heroJson = Resources.Load<TextAsset>("Config/heroes");
            if (abilityJson == null || upgradeJson == null || heroJson == null)
                throw new InvalidOperationException("Missing Resources/Config JSON files.");

            AbilityConfigList abilityList = JsonUtility.FromJson<AbilityConfigList>(abilityJson.text);
            UpgradeConfigList upgradeList = JsonUtility.FromJson<UpgradeConfigList>(upgradeJson.text);
            HeroConfigList heroList = JsonUtility.FromJson<HeroConfigList>(heroJson.text);
            abilities = new Dictionary<int, AbilityConfig>();
            foreach (AbilityConfig ability in abilityList.abilities)
                abilities.Add(ability.abilityId, ability);
            upgrades = new List<UpgradeConfig>(upgradeList.upgrades);
            upgradesById = new Dictionary<string, UpgradeConfig>(StringComparer.OrdinalIgnoreCase);
            foreach (UpgradeConfig upgrade in upgrades)
                upgradesById[upgrade.upgradeId] = upgrade;
            heroes = new Dictionary<int, HeroConfig>();
            foreach (HeroConfig hero in heroList.heroes)
                heroes.Add(hero.heroId, hero);
        }
    }

    public sealed class AbilityRuntime
    {
        public readonly AbilityConfig Config;
        public int Counter;
        public int ReadyAtMs;

        public AbilityRuntime(AbilityConfig config)
        {
            Config = config;
        }
    }

    public sealed class FighterState
    {
        public int Id;
        public string Name;
        public int MaxHealth;
        public int Health;
        public int Attack;
        public int Defense;
        public int AttackIntervalMs;
        public int DodgeBP;
        public int CritBP;
        public int CritDamageBP = 17500;
        public int BasicPowerBP = 10000;
        public int UltimatePowerBP = 10000;
        public int EnergyGainBP = 10000;
        public int BurnPowerBP = 10000;
        public int FrostPowerBP = 10000;
        public int CounterPowerBP = 10000;
        public int Energy;
        public int MaxEnergy = 100;
        public int FrostStacks;
        public int FrozenUntilMs;
        public int BonusDodgeBP;
        public int BonusDodgeUntilMs;
        public int HeroId;
        public int NextAttackMs;
        public readonly List<AbilityRuntime> Abilities = new List<AbilityRuntime>();
        public readonly List<BurnRuntime> Burns = new List<BurnRuntime>();
        public bool IsAlive => Health > 0;
    }

    public sealed class BurnRuntime
    {
        public int SourceId;
        public int Damage;
        public int TicksLeft;
        public int NextTickMs;
        public long ActionInstanceId;
    }

    public sealed class BattleViewEvent
    {
        public int Tick;
        public int Sequence;
        public BattleViewEventType Type;
        public int SourceUnitId;
        public int TargetUnitId;
        public int AbilityId;
        public string AbilityName;
        public int Value;
        public int HealthAfter;
        public int MaxHealth;
        public int EnergyAfter;
        public int MaxEnergy;
        public bool IsCritical;
        public bool IsLethal;
        public string Element;
        public string Note;
        public long ActionInstanceId;
        public bool PlayerWon;
    }

    public sealed class BattleResult
    {
        public readonly List<BattleViewEvent> Events = new List<BattleViewEvent>();
        public bool PlayerWon;
        public int DurationMs;
    }

    public sealed class BuildState
    {
        public int AttackBP = 10000;
        public int HealthBP = 10000;
        public int AttackSpeedBP = 10000;
        public int DodgeBP = 500;
        public int CritBP = 800;
        public int DefenseFlat;
        public int HeroId = 1;
        public int CritDamageBP = 17500;
        public int BasicPowerBP = 10000;
        public int UltimatePowerBP = 10000;
        public int EnergyGainBP = 10000;
        public int BurnPowerBP = 10000;
        public int FrostPowerBP = 10000;
        public int CounterPowerBP = 10000;
        public readonly List<int> AbilityIds = new List<int>();
        public readonly Dictionary<string, int> UpgradeRanks = new Dictionary<string, int>();

        public bool HasAbility(int id) => AbilityIds.Contains(id);

        public void Apply(UpgradeConfig config)
        {
            AttackBP += config.attackBP;
            HealthBP += config.healthBP;
            AttackSpeedBP += config.attackSpeedBP;
            DodgeBP += config.dodgeBP;
            CritBP += config.critBP;
            DefenseFlat += config.defenseFlat;
            CritDamageBP += config.critDamageBP;
            BasicPowerBP += config.basicPowerBP;
            UltimatePowerBP += config.ultimatePowerBP;
            EnergyGainBP += config.energyGainBP;
            BurnPowerBP += config.burnPowerBP;
            FrostPowerBP += config.frostPowerBP;
            CounterPowerBP += config.counterPowerBP;
            if (config.addAbilityId != 0 && !AbilityIds.Contains(config.addAbilityId))
                AbilityIds.Add(config.addAbilityId);

            UpgradeRanks.TryGetValue(config.upgradeId, out int rank);
            UpgradeRanks[config.upgradeId] = rank + 1;
        }
    }

    public sealed class CombatWorld
    {
        private const int TickMs = 100;
        private const int TimeLimitMs = 45000;

        private readonly System.Random random;
        private readonly BattleResult result = new BattleResult();
        private readonly FighterState player;
        private readonly FighterState enemy;
        private readonly BuildState opponentBuild;
        private int now;
        private int sequence;
        private long nextActionId = 100000;

        public CombatWorld(BuildState playerBuild, int round, int seed)
        {
            random = new System.Random(seed);
            player = CreatePlayer(playerBuild);
            enemy = CreateEnemy(round);
        }

        public CombatWorld(BuildState playerBuild, BuildState enemyBuild, int round, int seed)
        {
            random = new System.Random(seed);
            opponentBuild = enemyBuild;
            player = CreatePlayer(playerBuild);
            enemy = CreateEnemy(round);
        }

        public FighterState Player => player;
        public FighterState Enemy => enemy;

        public BattleResult Run()
        {
            Emit(BattleViewEventType.BattleStarted, player, enemy, note: "斗技开始");
            DispatchForAll(TriggerEventType.BattleStart, null, 0);

            for (now = 0; now <= TimeLimitMs && player.IsAlive && enemy.IsAlive; now += TickMs)
            {
                ProcessBurns(player);
                ProcessBurns(enemy);
                TryUltimate(player, enemy);
                TryUltimate(enemy, player);
                TryBasicAttack(player, enemy);
                TryBasicAttack(enemy, player);
            }

            if (player.IsAlive && enemy.IsAlive)
            {
                // 超时按剩余生命比例判定，避免拖延型构筑无限僵持。
                float playerRatio = player.Health / (float)player.MaxHealth;
                float enemyRatio = enemy.Health / (float)enemy.MaxHealth;
                if (playerRatio >= enemyRatio) Kill(enemy, player, "时间裁定");
                else Kill(player, enemy, "时间裁定");
            }

            result.PlayerWon = player.IsAlive;
            result.DurationMs = now;
            BattleViewEvent end = Emit(BattleViewEventType.BattleEnded,
                result.PlayerWon ? player : enemy,
                result.PlayerWon ? enemy : player,
                note: result.PlayerWon ? "本轮胜利" : "本轮失利");
            end.PlayerWon = result.PlayerWon;
            return result;
        }

        private FighterState CreatePlayer(BuildState build)
        {
            HeroConfig hero = GameConfig.Hero(build.HeroId);
            FighterState fighter = new FighterState
            {
                Id = 1,
                HeroId = hero.heroId,
                Name = hero.heroName,
                MaxHealth = hero.baseHealth * build.HealthBP / 10000,
                Attack = hero.baseAttack * build.AttackBP / 10000,
                Defense = hero.baseDefense + build.DefenseFlat,
                AttackIntervalMs = Mathf.Clamp(hero.attackIntervalMs * 10000 / Mathf.Max(3000, build.AttackSpeedBP), 350, 1800),
                DodgeBP = Mathf.Clamp(hero.dodgeBP + build.DodgeBP, 0, 6500),
                CritBP = Mathf.Clamp(hero.critBP + build.CritBP, 0, 8500),
                CritDamageBP = build.CritDamageBP,
                BasicPowerBP = build.BasicPowerBP,
                UltimatePowerBP = build.UltimatePowerBP,
                EnergyGainBP = build.EnergyGainBP,
                BurnPowerBP = build.BurnPowerBP,
                FrostPowerBP = build.FrostPowerBP,
                CounterPowerBP = build.CounterPowerBP,
                NextAttackMs = 450
            };
            fighter.Health = fighter.MaxHealth;
            fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(hero.passiveAbilityId)));
            fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(hero.ultimateAbilityId)));
            foreach (int id in build.AbilityIds)
                if (id != hero.passiveAbilityId && id != hero.ultimateAbilityId)
                    fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(id)));
            return fighter;
        }

        private FighterState CreateEnemy(int round)
        {
            int heroId = opponentBuild != null ? opponentBuild.HeroId : 1 + ((round + random.Next(0, 6)) % 6);
            HeroConfig hero = GameConfig.Hero(heroId);
            int healthBP = opponentBuild?.HealthBP ?? 10000;
            int attackBP = opponentBuild?.AttackBP ?? 10000;
            int speedBP = opponentBuild?.AttackSpeedBP ?? 10000;
            FighterState fighter = new FighterState
            {
                Id = 2,
                HeroId = hero.heroId,
                Name = hero.heroName,
                MaxHealth = opponentBuild != null ? hero.baseHealth * healthBP / 10000 : hero.baseHealth + round * 105,
                Attack = opponentBuild != null ? hero.baseAttack * attackBP / 10000 : hero.baseAttack + round * 11,
                Defense = hero.baseDefense + (opponentBuild?.DefenseFlat ?? round * 2),
                AttackIntervalMs = opponentBuild != null
                    ? Mathf.Clamp(hero.attackIntervalMs * 10000 / Mathf.Max(3000, speedBP), 350, 1800)
                    : Mathf.Max(480, hero.attackIntervalMs - round * 45),
                DodgeBP = hero.dodgeBP + (opponentBuild?.DodgeBP ?? round * 100),
                CritBP = hero.critBP + (opponentBuild?.CritBP ?? round * 150),
                CritDamageBP = opponentBuild?.CritDamageBP ?? 17500 + round * 300,
                BasicPowerBP = opponentBuild?.BasicPowerBP ?? 10000,
                UltimatePowerBP = opponentBuild?.UltimatePowerBP ?? 10000 + round * 650,
                EnergyGainBP = opponentBuild?.EnergyGainBP ?? 10000,
                BurnPowerBP = opponentBuild?.BurnPowerBP ?? 10000 + round * 500,
                FrostPowerBP = opponentBuild?.FrostPowerBP ?? 10000 + round * 500,
                CounterPowerBP = opponentBuild?.CounterPowerBP ?? 10000,
                NextAttackMs = 650
            };
            fighter.Health = fighter.MaxHealth;
            fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(hero.passiveAbilityId)));
            fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(hero.ultimateAbilityId)));
            if (opponentBuild != null)
            {
                foreach (int id in opponentBuild.AbilityIds)
                    if (id != hero.passiveAbilityId && id != hero.ultimateAbilityId)
                        fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(id)));
            }
            else
            {
                int[] schoolAbilities = { 100701, 100201, 100801, 100301, 100501, 100601 };
                if (round >= 2) fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(schoolAbilities[heroId - 1])));
                if (round >= 4) fighter.Abilities.Add(new AbilityRuntime(GameConfig.Ability(100101)));
            }
            return fighter;
        }

        private void TryBasicAttack(FighterState source, FighterState target)
        {
            if (!source.IsAlive || !target.IsAlive || now < source.NextAttackMs || now < source.FrozenUntilMs) return;
            source.NextAttackMs += source.AttackIntervalMs + source.FrostStacks * 32;
            long actionId = ++nextActionId;
            Emit(BattleViewEventType.AttackStarted, source, target, actionId: actionId, note: "普通攻击");
            int basicDamage = source.Attack * source.BasicPowerBP / 10000;
            bool hit = ResolveDamage(source, target, basicDamage, 0, "普通攻击", "Physical", actionId, true, true, 0);
            if (hit && source.IsAlive)
            {
                GainEnergy(source, 12, actionId);
                BattleViewEvent context = new BattleViewEvent
                {
                    SourceUnitId = source.Id,
                    TargetUnitId = target.Id,
                    Value = source.Attack,
                    ActionInstanceId = actionId
                };
                Dispatch(source, TriggerEventType.AfterBasicAttack, context, 0);
            }
        }

        private void TryUltimate(FighterState source, FighterState target)
        {
            if (!source.IsAlive || !target.IsAlive || source.Energy < source.MaxEnergy || now < source.FrozenUntilMs)
                return;
            BattleViewEvent context = new BattleViewEvent
            {
                SourceUnitId = source.Id,
                TargetUnitId = target.Id,
                Value = source.Energy,
                ActionInstanceId = ++nextActionId
            };
            Dispatch(source, TriggerEventType.EnergyFull, context, 0);
        }

        private bool ResolveDamage(FighterState source, FighterState target, int rawDamage, int abilityId,
            string abilityName, string element, long actionId, bool canDodge, bool canCrit, int depth,
            bool forceCritical = false)
        {
            if (!source.IsAlive || !target.IsAlive) return false;
            int effectiveDodge = target.DodgeBP + (now < target.BonusDodgeUntilMs ? target.BonusDodgeBP : 0);
            if (canDodge && Roll(Mathf.Clamp(effectiveDodge, 0, 7000)))
            {
                BattleViewEvent dodge = Emit(BattleViewEventType.DodgeSucceeded, source, target, abilityId,
                    abilityName, 0, element, actionId, "闪避");
                Dispatch(target, TriggerEventType.DodgeSucceeded, dodge, depth + 1);
                return false;
            }

            bool critical = forceCritical || (canCrit && Roll(source.CritBP));
            int damage = Mathf.Max(1, rawDamage - target.Defense);
            if (critical) damage = damage * source.CritDamageBP / 10000;
            target.Health = Mathf.Max(0, target.Health - damage);
            GainEnergy(target, 5, actionId);

            BattleViewEvent damageEvent = Emit(BattleViewEventType.DamageResolved, source, target, abilityId,
                abilityName, damage, element, actionId, critical ? "暴击" : string.Empty);
            damageEvent.HealthAfter = target.Health;
            damageEvent.MaxHealth = target.MaxHealth;
            damageEvent.IsCritical = critical;
            damageEvent.IsLethal = target.Health == 0;
            DispatchForAll(TriggerEventType.DamageResolved, damageEvent, depth + 1);

            if (target.Health == 0)
                Emit(BattleViewEventType.UnitDied, source, target, abilityId, abilityName, damage, element, actionId);
            return true;
        }

        private void DispatchForAll(TriggerEventType trigger, BattleViewEvent context, int depth)
        {
            if (player.IsAlive) Dispatch(player, trigger, context, depth);
            if (enemy.IsAlive) Dispatch(enemy, trigger, context, depth);
        }

        private void Dispatch(FighterState owner, TriggerEventType trigger, BattleViewEvent context, int depth)
        {
            if (depth > 8 || !owner.IsAlive) return;
            for (int i = 0; i < owner.Abilities.Count; i++)
            {
                AbilityRuntime runtime = owner.Abilities[i];
                AbilityConfig config = runtime.Config;
                if (config.Trigger != trigger || !ConditionPasses(config.Condition, owner, context)) continue;

                runtime.Counter++;
                if (runtime.Counter < Mathf.Max(1, config.triggerCount)) continue;
                runtime.Counter = 0;
                if (now < runtime.ReadyAtMs || !Roll(config.triggerChanceBP)) continue;

                FighterState target = SelectTarget(config.Target, owner, context);
                if (target == null || !target.IsAlive) continue;
                if (config.energyCost > 0 && owner.Energy < config.energyCost) continue;
                if (config.energyCost > 0)
                {
                    owner.Energy -= config.energyCost;
                    EmitEnergy(owner, actionId: context?.ActionInstanceId ?? 0);
                }
                runtime.ReadyAtMs = now + config.internalCooldownMs;
                long actionId = ++nextActionId;
                Emit(BattleViewEventType.AbilityStarted, owner, target, config.abilityId,
                    config.abilityName, 0, TagElement(config.tags), actionId, config.description);

                if (config.effects == null) continue;
                foreach (EffectConfig effect in config.effects)
                    ExecuteEffect(owner, target, config, effect, context, actionId, depth + 1);

                if (config.isUltimate)
                {
                    BattleViewEvent ultimateContext = new BattleViewEvent
                    {
                        SourceUnitId = owner.Id,
                        TargetUnitId = target.Id,
                        AbilityId = config.abilityId,
                        AbilityName = config.abilityName,
                        ActionInstanceId = actionId
                    };
                    Dispatch(owner, TriggerEventType.AfterUltimate, ultimateContext, depth + 1);
                }
            }
        }

        private void ExecuteEffect(FighterState owner, FighterState target, AbilityConfig ability,
            EffectConfig effect, BattleViewEvent context, long actionId, int depth)
        {
            if (!Enum.TryParse(effect.effectType, true, out EffectType effectType)) return;
            switch (effectType)
            {
                case EffectType.Damage:
                    int power = ability.isUltimate ? owner.UltimatePowerBP :
                        ability.tags != null && ability.tags.IndexOf("Counter", StringComparison.OrdinalIgnoreCase) >= 0
                            ? owner.CounterPowerBP : 10000;
                    int raw = owner.Attack * effect.coefficientBP / 10000 * power / 10000 + effect.flatValue;
                    ResolveDamage(owner, target, raw, ability.abilityId, ability.abilityName,
                        effect.element, actionId, false, true, depth);
                    break;

                case EffectType.RepeatDamage:
                    int repeatedPower = ability.isUltimate ? owner.UltimatePowerBP :
                        ability.tags != null && ability.tags.IndexOf("Counter", StringComparison.OrdinalIgnoreCase) >= 0
                            ? owner.CounterPowerBP : 10000;
                    int repeatedRaw = owner.Attack * effect.coefficientBP / 10000 * repeatedPower / 10000 + effect.flatValue;
                    int repeats = Mathf.Clamp(effect.repeatCount, 1, 12);
                    for (int i = 0; i < repeats && target.IsAlive; i++)
                        ResolveDamage(owner, target, repeatedRaw, ability.abilityId, ability.abilityName,
                            effect.element, actionId, false, true, depth, effect.guaranteedCritical);
                    break;

                case EffectType.HealFromEvent:
                    if (context == null || context.Value <= 0) return;
                    int heal = Mathf.Max(1, context.Value * effect.coefficientBP / 10000 + effect.flatValue);
                    int before = owner.Health;
                    owner.Health = Mathf.Min(owner.MaxHealth, owner.Health + heal);
                    heal = owner.Health - before;
                    if (heal <= 0) return;
                    BattleViewEvent healEvent = Emit(BattleViewEventType.HealResolved, owner, owner,
                        ability.abilityId, ability.abilityName, heal, "Heal", actionId);
                    healEvent.HealthAfter = owner.Health;
                    healEvent.MaxHealth = owner.MaxHealth;
                    break;

                case EffectType.AddBurn:
                    int tickDamage = Mathf.Max(1, owner.Attack * effect.coefficientBP / 10000 * owner.BurnPowerBP / 10000 + effect.flatValue);
                    target.Burns.Add(new BurnRuntime
                    {
                        SourceId = owner.Id,
                        Damage = tickDamage,
                        TicksLeft = 3,
                        NextTickMs = now + 1000,
                        ActionInstanceId = actionId
                    });
                    Emit(BattleViewEventType.BuffAdded, owner, target, ability.abilityId,
                        ability.abilityName, 3, "Fire", actionId, "灼烧 · 3层");
                    break;

                case EffectType.AddFrost:
                    int added = Mathf.Max(1, effect.flatValue == 0 ? 1 : effect.flatValue);
                    added = Mathf.Max(1, Mathf.CeilToInt(added * owner.FrostPowerBP / 10000f));
                    target.FrostStacks += added;
                    Emit(BattleViewEventType.BuffAdded, owner, target, ability.abilityId,
                        ability.abilityName, target.FrostStacks, "Frost", actionId, $"寒霜 · {target.FrostStacks}层");
                    if (target.FrostStacks >= 6)
                    {
                        target.FrostStacks = 0;
                        target.FrozenUntilMs = Mathf.Max(target.FrozenUntilMs, now + 900);
                        Emit(BattleViewEventType.BuffAdded, owner, target, ability.abilityId,
                            ability.abilityName, 900, "Frost", actionId, "冻结!");
                        int shatter = Mathf.Max(1, owner.Attack * 5500 / 10000 * owner.FrostPowerBP / 10000);
                        ResolveDamage(owner, target, shatter, ability.abilityId, "寒霜碎裂",
                            "Frost", actionId, false, false, depth);
                    }
                    break;

                case EffectType.GainEnergy:
                    GainEnergy(owner, effect.flatValue, actionId);
                    break;

                case EffectType.DetonateBurn:
                    int burst = 0;
                    foreach (BurnRuntime burn in target.Burns)
                        burst += burn.Damage * burn.TicksLeft;
                    target.Burns.Clear();
                    if (burst > 0)
                    {
                        ResolveDamage(owner, target, burst, ability.abilityId, ability.abilityName,
                            "Fire", actionId, false, false, depth);
                        Emit(BattleViewEventType.BuffRemoved, owner, target, ability.abilityId,
                            ability.abilityName, 0, "Fire", actionId, "引爆灼烧");
                    }
                    break;

                case EffectType.TemporaryDodge:
                    owner.BonusDodgeBP = Mathf.Max(owner.BonusDodgeBP, effect.coefficientBP);
                    owner.BonusDodgeUntilMs = Mathf.Max(owner.BonusDodgeUntilMs, now + Mathf.Max(500, effect.durationMs));
                    Emit(BattleViewEventType.BuffAdded, owner, owner, ability.abilityId,
                        ability.abilityName, effect.coefficientBP, "Dodge", actionId, "幻影步");
                    break;
            }
        }

        private void GainEnergy(FighterState fighter, int baseAmount, long actionId)
        {
            if (!fighter.IsAlive || baseAmount <= 0) return;
            int amount = Mathf.Max(1, baseAmount * fighter.EnergyGainBP / 10000);
            int before = fighter.Energy;
            fighter.Energy = Mathf.Clamp(fighter.Energy + amount, 0, fighter.MaxEnergy);
            if (fighter.Energy != before) EmitEnergy(fighter, actionId);
        }

        private void EmitEnergy(FighterState fighter, long actionId)
        {
            BattleViewEvent energy = Emit(BattleViewEventType.EnergyChanged, fighter, fighter,
                value: fighter.Energy, element: "Arcane", actionId: actionId, note: "能量");
            energy.EnergyAfter = fighter.Energy;
            energy.MaxEnergy = fighter.MaxEnergy;
        }

        private void ProcessBurns(FighterState target)
        {
            if (!target.IsAlive) return;
            for (int i = target.Burns.Count - 1; i >= 0; i--)
            {
                BurnRuntime burn = target.Burns[i];
                if (now < burn.NextTickMs) continue;
                FighterState source = burn.SourceId == player.Id ? player : enemy;
                if (source.IsAlive)
                    ResolveDamage(source, target, burn.Damage, 100601, "余烬印记", "Fire",
                        burn.ActionInstanceId, false, false, 1);
                burn.TicksLeft--;
                burn.NextTickMs += 1000;
                if (burn.TicksLeft <= 0 || !target.IsAlive)
                {
                    target.Burns.RemoveAt(i);
                    Emit(BattleViewEventType.BuffRemoved, source, target, 100601,
                        "余烬印记", 0, "Fire", burn.ActionInstanceId, "灼烧结束");
                }
            }
        }

        private static bool ConditionPasses(ConditionType condition, FighterState owner, BattleViewEvent context)
        {
            switch (condition)
            {
                case ConditionType.EventTargetIsOwner:
                    return context != null && context.TargetUnitId == owner.Id;
                case ConditionType.EventSourceIsOwner:
                    return context != null && context.SourceUnitId == owner.Id;
                case ConditionType.EventWasCritical:
                    return context != null && context.SourceUnitId == owner.Id && context.IsCritical;
                default:
                    return true;
            }
        }

        private FighterState SelectTarget(TargetRule rule, FighterState owner, BattleViewEvent context)
        {
            switch (rule)
            {
                case TargetRule.Self:
                    return owner;
                case TargetRule.EventSource:
                    return context == null ? null : Fighter(context.SourceUnitId);
                default:
                    return owner.Id == player.Id ? enemy : player;
            }
        }

        private FighterState Fighter(int id)
        {
            if (id == player.Id) return player;
            if (id == enemy.Id) return enemy;
            return null;
        }

        private void Kill(FighterState target, FighterState source, string note)
        {
            target.Health = 0;
            BattleViewEvent damage = Emit(BattleViewEventType.DamageResolved, source, target,
                value: 0, element: "Void", actionId: ++nextActionId, note: note);
            damage.HealthAfter = 0;
            damage.MaxHealth = target.MaxHealth;
            damage.IsLethal = true;
            Emit(BattleViewEventType.UnitDied, source, target, note: note);
        }

        private bool Roll(int basisPoints)
        {
            return basisPoints >= 10000 || (basisPoints > 0 && random.Next(0, 10000) < basisPoints);
        }

        private BattleViewEvent Emit(BattleViewEventType type, FighterState source, FighterState target,
            int abilityId = 0, string abilityName = null, int value = 0, string element = null,
            long actionId = 0, string note = null)
        {
            BattleViewEvent item = new BattleViewEvent
            {
                Tick = now,
                Sequence = ++sequence,
                Type = type,
                SourceUnitId = source?.Id ?? 0,
                TargetUnitId = target?.Id ?? 0,
                AbilityId = abilityId,
                AbilityName = abilityName,
                Value = value,
                Element = element,
                ActionInstanceId = actionId,
                Note = note
            };
            result.Events.Add(item);
            return item;
        }

        private static string TagElement(string tags)
        {
            if (string.IsNullOrEmpty(tags)) return "Physical";
            if (tags.IndexOf("Lightning", StringComparison.OrdinalIgnoreCase) >= 0) return "Lightning";
            if (tags.IndexOf("Fire", StringComparison.OrdinalIgnoreCase) >= 0) return "Fire";
            if (tags.IndexOf("Heal", StringComparison.OrdinalIgnoreCase) >= 0) return "Heal";
            return "Physical";
        }
    }
}
