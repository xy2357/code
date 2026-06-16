namespace Question26;

class Program
{
    static void Main(string[] args)
    {
        Player player = new Player("player", 120);
        Monster monster = new Monster("monster", 100);

        player.Died += OnDied;
        monster.Died += OnDied;

        Attack(player, 200);
        Attack(monster, 110);
    }

    static void Attack(IDamageable target, int damage)
    {
        target.TakeDamage(damage);
    }

    static void OnDied(object? sender, EventArgs e)
    {
        DiedEventArgs target = (DiedEventArgs)e;
        Console.WriteLine($"{target.Name}受到{target.Damage}伤害，最终血量{target.Hp}");
    }
}

interface IDamageable
{
    void TakeDamage(int damage);
}

class Player : IDamageable
{
    private string _name;
    private int _hp;
    private bool _isDead = false;
    public Player(string name, int hp)
    {
        _name = name;
        _hp = hp;
    }

    public event EventHandler? Died;
    
    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        _hp -= damage;
        if (_hp <= 0)
        {
            _isDead = true;
            DiedEventArgs e = new DiedEventArgs();
            e.Name = _name;
            e.Hp = _hp;
            e.Damage = damage;
            Died?.Invoke(this,e);
        }
    }
}

class Monster : IDamageable
{
    private string _name;
    private int _hp;
    private bool _isDead = false;
    public Monster(string name, int hp)
    {
        _name = name;
        _hp = hp;
    }

    public event EventHandler? Died;
    
    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        _hp -= damage;
        if (_hp <= 0)
        {
            _isDead = true;
            DiedEventArgs e = new DiedEventArgs();
            e.Name = _name;
            e.Hp = _hp;
            e.Damage = damage;
            Died?.Invoke(this,e);
        }
    }
}

class DiedEventArgs : EventArgs
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int Damage { get; set; }
}