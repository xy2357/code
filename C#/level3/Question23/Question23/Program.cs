namespace Question23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player("xy", 100);
            player.Damage += OnDamage;
            player.TakeDamage(30);
        }

        static void OnDamage(int damage,int hp)
        {
            Console.WriteLine($"玩家受到了{damage}点伤害,剩余血量{hp}");
        }
    }

    class Player
    {
        private string _name;
        private int _hp;

        public Player(string name, int hp)
        {
            _name = name;
            _hp = hp;
        }

        public event Action<int,int> Damage;

        public void TakeDamage(int damage)
        {
            _hp -= damage;
            Damage?.Invoke(damage, _hp);
        }
    }
}
