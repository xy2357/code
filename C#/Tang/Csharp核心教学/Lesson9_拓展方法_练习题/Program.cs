namespace Lesson9_拓展方法_练习题
{

    static class IntExtension
    {
        public static int CalSquare(this int num)
        {
            return num * num;
        }
    }

    class Player
    {
        private int hp;
        public Player(string name,int hp, int atk, int def)
        {
            Name = name;
            Hp = hp;
            Atk = atk;
            Def = def;
        }

        public string Name{ get; set; }

        public int Hp
        {
            get
            {
                return hp;
            }
            set
            {
                hp = Math.Max(0, value);
            }
        }

        public int Atk{ get; set; }

        public int Def{ get; set; }

        public void Attack(Player other)
        {
            other.Hp -= (Atk - other.Def);
        }

        public void OnAttack(Player other)
        {
            Hp -= (other.Atk - Def);
        }
    }

    static class PlayerExtension
    {
        public static void Suicide(this Player p)
        {
            p.Hp = 0;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //int a = 5;
            //Console.WriteLine(5.CalSquare());

            Player p1 = new Player("小王", 100, 20, 10);
            Player p2 = new Player("二号", 100, 18, 12);
            p1.Attack(p2);
            Console.WriteLine(p2.Hp);
            p2.Suicide();
            Console.WriteLine(p2.Hp);
        }
    }
}
