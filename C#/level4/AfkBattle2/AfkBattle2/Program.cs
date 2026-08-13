namespace AfkBattle2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Character hero1 = new Character("hero1", 100, 20, 8, 20, 20);
            Character hero2 = new Character("hero2", 100, 20, 8, 20, 20);
            Character hero3 = new Character("hero3", 100, 20, 8, 20, 20);

            Character enemy1 = new Character("enemy1", 50, 10, 5, 10, 10);
            Character enemy2 = new Character("enemy2", 50, 10, 5, 10, 10);
            Character enemy3 = new Character("enemy3", 50, 10, 5, 10, 10);

            List<Character> enemies = new List<Character> { enemy1, enemy2, enemy3 };

            Battle battle = new Battle(hero1, enemies);

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

        public Character(string name, int hp, int attack, int defense, int criticalRate, int dodgeRate)
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
        private Character hero;
        private List<Character> enemies;
        private int round = 1;

        public Battle(Character hero, List<Character> enemies)
        {
            this.hero = hero;
            this.enemies = enemies;
        }

        public void Start()
        {
            while (!hero.IsDead)
            {
                Console.WriteLine($"=====第{round}回合=====");

                Character? target = FindTarget();

                if (target == null)
                {
                    Console.WriteLine("敌方全灭！");
                    Console.WriteLine($"{hero.Name}获胜！");
                    break;
                }
                hero.AttackTarget(target);

                if (IsTeamDead())
                {
                    Console.WriteLine("敌方全灭！");
                    Console.WriteLine($"{hero.Name}获胜！");
                    break;
                }

                foreach (Character enemy in enemies)
                {
                    if (enemy.IsDead)
                    {
                        continue;
                    }
                    enemy.AttackTarget(hero);

                    if (hero.IsDead)
                    {
                        Console.WriteLine($"{hero.Name}已死亡！");
                        Console.WriteLine($"敌方获胜！");
                        return;
                    }
                }
                //if (target.IsDead)
                //{
                //    continue;
                //}

                //target.AttackTarget(hero);

                //if (hero.IsDead)
                //{
                //    Console.WriteLine($"{hero.Name}已死亡！");
                //    Console.WriteLine($"敌方获胜！");
                //}
                round++;
            }
        }

        public Character? FindTarget()
        {
            Character? nowEnemy = null;
            foreach (Character enemy in enemies)
            {
                if (enemy.IsDead)
                {
                    continue;
                }

                if (nowEnemy == null || enemy.Hp < nowEnemy.Hp)
                {
                    nowEnemy = enemy;
                }
            }
            return nowEnemy;
        }

        public bool IsTeamDead()
        {
            foreach (Character enemy in enemies)
            {
                if (!enemy.IsDead)
                {
                    return false;
                }
            }
            return true;
        }
    }
}