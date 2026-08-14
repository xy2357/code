using System.Xml.Linq;

namespace AfkBattle2
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Character hero1 = new Character("hero1", 100, 20, 8, 20, 20, Character.CharacterRole.Attacker);
            Character hero2 = new Character("hero2", 100, 20, 8, 20, 20, Character.CharacterRole.Attacker);
            Character hero3 = new Character("hero3", 100, 20, 8, 20, 20, Character.CharacterRole.Healer);


            Character enemy1 = new Character("enemy1", 50, 10, 5, 10, 10, Character.CharacterRole.Attacker);
            Character enemy2 = new Character("enemy2", 50, 10, 5, 10, 10, Character.CharacterRole.Attacker);
            Character enemy3 = new Character("enemy3", 50, 10, 5, 10, 10, Character.CharacterRole.Attacker);

            List<Character> heroes = new List<Character> { hero1, hero2, hero3 };
            List<Character> enemies = new List<Character> { enemy1, enemy2, enemy3 };

            Battle battle = new Battle(heroes, enemies);

            battle.Start();
        }
    }

    class Character
    {
        public string Name { get; set; }
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public bool IsDead => Hp <= 0;
        public int CriticalRate { get; set; }
        public int DodgeRate { get; set; }
        public CharacterRole Role { get; set; }

        public Character(string name, int hp, int attack, int defense, int criticalRate, int dodgeRate, CharacterRole role)
        {
            Name = name;
            Hp = hp;
            MaxHp = hp;
            Attack = attack;
            Defense = defense;
            CriticalRate = criticalRate;
            DodgeRate = dodgeRate;
            Role = role;
        }

        public enum CharacterRole
        {
            Attacker,
            Healer
        }

        public void AttackTarget(Character target)
        {
            if (target.IsDodge())
            {
                Console.WriteLine($"{Name}攻击了{target.Name}！");
                Console.WriteLine($"{target.Name}闪避了攻击！");
                return;
            }

            int damage = Math.Max(1, Attack - target.Defense);

            if (IsCritical())
            {
                Console.WriteLine($"{Name}暴击了！");
                damage *= 2;
            }

            target.TakeDamage(damage);

            Console.WriteLine($"{Name}攻击了{target.Name}");
            Console.WriteLine($"造成了{damage}点伤害");
            Console.WriteLine($"{target.Name}剩余生命值：{target.Hp}/{target.MaxHp}");

            if (target.IsDead)
            {
                Console.WriteLine($"{target.Name}已死亡！");
            }
        }

        public void Heal(int amount)
        {
            if (IsDead)
            {
                Console.WriteLine($"{Name}已死亡，不能治疗！");
                return;
            }

            int oldHp = Hp;

            Hp = Math.Min(MaxHp, Hp + amount);

            int realHeal = Hp - oldHp;

            Console.WriteLine($"{Name}恢复了{realHeal}点生命！");
            Console.WriteLine($"{Name}当前生命值：{Hp}/{MaxHp}");
        }

        public void HealTarget(Character target)
        {
            Console.WriteLine($"{Name}治疗了{target.Name}");
            target.Heal(Attack);
        }

        public void TakeDamage(int damage)
        {
            Hp = Math.Max(0, Hp - damage);
        }

        public bool IsDodge()
        {
            int number = Random.Shared.Next(100);
            return number < DodgeRate;
        }

        public bool IsCritical()
        {
            int number = Random.Shared.Next(100);
            return number < CriticalRate;
        }
    }

    class Battle
    {
        private List<Character> heros;
        private List<Character> enemies;
        private int round = 1;

        public Battle(List<Character> heros, List<Character> enemies)
        {
            this.heros = heros;
            this.enemies = enemies;
        }

        public void Start()
        {
            while (!IsTeamDead(heros) && !IsTeamDead(enemies))
            {
                Console.WriteLine($"=====第{round}回合=====");

                TeamAttack(heros,enemies);

                if (IsTeamDead(enemies))
                {
                    Console.WriteLine("敌方全灭！");
                    Console.WriteLine("我方获胜！");
                    return;
                }

                TeamAttack(enemies,heros);

                if (IsTeamDead(heros))
                {
                    Console.WriteLine("我方全灭！");
                    Console.WriteLine("敌方获胜！");
                    return;
                }
                round++;
            }
        }

        //攻击查找目标（最低血量目标）
        public Character? FindAttackTarget(List<Character> team)
        {
            Character? nowMember = null;
            foreach (Character member in team)
            {
                if (member.IsDead)
                {
                    continue;
                }

                if (nowMember == null || member.Hp < nowMember.Hp)
                {
                    nowMember = member;
                }
            }
            return nowMember;
        }

        //治疗查找目标（最低百分比血量目标）
        public Character? FindHealTarget(List<Character> team)
        {
            Character? nowMember = null;
            float minHpPrecent = 1f;
            
            foreach (Character member in team)
            {
                if (member.IsDead)
                {
                    continue;
                }

                if (member.Hp == member.MaxHp)
                {
                    continue;
                }

                float nowMinHpPrecent = (float)member.Hp / member.MaxHp;
                if (nowMember == null || nowMinHpPrecent < minHpPrecent)
                {
                    nowMember = member;
                    minHpPrecent = nowMinHpPrecent;
                }
            }
            return nowMember;
        }

        public void TeamAttack(List<Character> team,List<Character> targetTeam)
        {
            foreach (Character member in team)
            {
                if (member.IsDead)
                {
                    continue;
                }

                // 对方已经全灭，本队停止行动
                if (IsTeamDead(targetTeam))
                {
                    return;
                }

                if (member.Role == Character.CharacterRole.Healer)
                {
                    Character? healTarget = FindHealTarget(team);

                    if (healTarget == null)
                    {
                        Console.WriteLine($"{member.Name}没有需要治疗的目标！");
                        continue;
                    }

                    member.HealTarget(healTarget);

                }
                else
                {
                    Character? target = FindAttackTarget(targetTeam);

                    if (target == null)
                    {
                        return;
                    }
                    member.AttackTarget(target);
                }
            }
        }

        public bool IsTeamDead(List<Character> team)
        {
            foreach (Character member in team)
            {
                if (!member.IsDead)
                {
                    return false;
                }
            }
            return true;
        }
    }
}