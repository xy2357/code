namespace AfkBattlePrototype
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Character hero1 = new Character("hero1", 100, 20, 10, 20, 10);
            Character hero2 = new Character("hero2", 80, 15, 12, 50, 30);

            Battle battle = new Battle(hero1, hero2);

            battle.Start();
        }

        public Character? FindTarget(List<Character> enemies)
        {
            Character? nowEnemy = null;
            foreach (Character enemy in enemies)
            {
                if (enemy.Hp < 0)
                {
                    continue;
                }
                if (nowEnemy == null ||enemy.Hp < nowEnemy.Hp)
                {
                    nowEnemy = enemy;
                }
            }

            return nowEnemy;
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

        public Character(string name, int hp, int attack, int defense, int criticalRate,int dodgeRate)
        {
            Name = name;
            Hp = hp;
            MaxHp = hp;
            Attack = attack;
            Defense = defense;
            CriticalRate = criticalRate;
            DodgeRate = dodgeRate;
        }

        public void AttackTarget(Character target)
        {
            //闪避判定
            if (target.IsDodge())
            {
                Console.WriteLine($"{Name}攻击了{target.Name}");
                Console.WriteLine($"{target.Name}闪避了");
                return;
            }

            int damage = Math.Max(1, Attack - target.Defense);

            //暴击判定
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

        public void TakeDamage(int damage)
        {
            Hp = Math.Max(0, Hp - damage);
        }

        public bool IsCritical()
        {
            //Random random = new Random();
            //int number = random.Next(1, 101);
            //if (number <= CriticalRate)
            //{
            //    return true;
            //}
            //return false;

            int number = Random.Shared.Next(100);
            return number < CriticalRate;
        }

        public bool IsDodge()
        {
            int number = Random.Shared.Next(100);
            return number < DodgeRate;
        }
    }

    class Battle
    {
        private Character hero1;
        private Character hero2;

        public Battle(Character hero1,Character hero2)
        {
            this.hero1 = hero1;
            this.hero2 = hero2;
        }

        public void Start()
        {
            while (!hero1.IsDead && !hero2.IsDead)
            {
                hero1.AttackTarget(hero2);

                if (hero2.IsDead)
                {
                    break;
                }

                hero2.AttackTarget(hero1);
            }
        }
    }
}
