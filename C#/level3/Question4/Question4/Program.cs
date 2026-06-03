namespace Question4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player  = new Player();
            Monster monster = new Monster();

            LetItMove(player);
            LetItMove(monster);

        }

        static void LetItMove(IMovable movable)
        {
            movable.Move();
        }
    }

    interface IMovable
    {
        void Move();
    }

    class Player : IMovable
    {
        public void Move()
        {
            Console.WriteLine("玩家移动");
        }
    }

    class Monster : IMovable
    {
        public void Move()
        {
            Console.WriteLine("怪物移动");
        }
    }
}

