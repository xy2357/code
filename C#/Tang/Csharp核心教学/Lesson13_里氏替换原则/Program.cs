namespace Lesson13_里氏替换原则
{
    class Monster
    {

    }

    class Boss : Monster
    {
        public void Skill()
        {
            Console.WriteLine("我会放技能");
        }
    }

    class Gobin : Monster
    {
        public void Attack()
        {
            Console.WriteLine("我会攻击");
        }
    }

    class Weapon
    {
        public string Name { get; }
        public Weapon(string name)
        {
            Name = name;
        }
    }

    class SubmachineGun : Weapon
    {
        public SubmachineGun() : base("冲锋枪")
        {

        }
    }

    class ShotGun : Weapon
    {
        public ShotGun() : base("霰弹枪")
        {

        }
    }

    class Pistol : Weapon
    {
        public Pistol() : base("手枪")
        {

        }
    }

    class Dagger : Weapon
    {
        public Dagger() : base("匕首")
        {

        }
    }

    class Player
    {
        public Weapon Weapon { get; private set; } = new Dagger();

        public void PickUpWeapon(Weapon weapon)
        {
            this.Weapon = weapon;
            Console.WriteLine("拾取到武器{0}",this.Weapon.Name);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 题目一
            Monster[] monsters = new Monster[10];
            Random r = new Random();

            for (int i = 0; i < monsters.Length; i++)
            {
                int j = r.Next(0, 2);
                if (j == 1)
                {
                    monsters[i] = new Boss();
                }
                else
                {
                    monsters[i] = new Gobin();
                }
            }

            foreach (var monster in monsters)
            {
                if (monster is Boss)
                {
                    (monster as Boss).Skill();
                }
                else
                {
                    (monster as Gobin).Attack();
                }
            }
            #endregion


            #region 题目二
            Player player = new Player();

            Console.WriteLine("初始武器：{0}", player.Weapon.Name);

            SubmachineGun gun = new SubmachineGun();
            player.PickUpWeapon(gun);

            Console.WriteLine("当前武器：{0}", player.Weapon.Name);

            player.PickUpWeapon(new Pistol());
            player.PickUpWeapon(new Dagger());
            #endregion
        }
    }
}
