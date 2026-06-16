namespace Question33
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player(100);
            Monster monster = new Monster(80);

            player.Died += OnAnyObjectDied;
            monster.Died += OnAnyObjectDied;

            player.TakeDamage(200);
            monster.TakeDamage(200);
        }

        static void OnAnyObjectDied(object? sender, EventArgs e)
        {
            Console.WriteLine("有对象死了");
        }
    }

    interface IKillable
    {
        event EventHandler? Died;
        void TakeDamage(int damage);
    }

    class Player : IKillable
    {
        private int _hp;
        private bool _isDead = false;
        public Player(int hp)
        {
            _hp = hp;
        }

        public event EventHandler? Died;

        public void TakeDamage(int damage)
        {
            if (_isDead){ return; }
            _hp -= damage;
            if ( _hp <=0 )
            {
                _isDead = true;
                Died?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    class Monster : IKillable
    {
        private int _hp;
        private bool _isDead = false;
        public Monster(int hp)
        {
            _hp = hp;
        }

        public event EventHandler? Died;

        public void TakeDamage(int damage)
        {
            if (_isDead) { return; }
            _hp -= damage;
            if (_hp <= 0)
            {
                _isDead = true;
                Died?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
