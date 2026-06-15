namespace Question22;

class Program
{
    static void Main(string[] args)
    {
        Player player = new Player("Peter");
        player.HpChanged += OnHpChanged;
        player.TakeDamage(10);
        player.TakeDamage(20);
        player.TakeDamage(30);
    }

    static void OnHpChanged(object? sender, HpChangedEventArgs e)
    {
        Console.WriteLine($"旧血量：{e.OldHp}");
        Console.WriteLine($"新血量：{e.NewHp}");
        Console.WriteLine($"伤害：{e.Damage}");
    }
}

class Player
{
    private string _name;
    private int _hp = 100;

    public Player(string name)
    {
        _name = name;
    }

    public event EventHandler<HpChangedEventArgs>? HpChanged;

    public void TakeDamage(int damage)
    {
        int OldHp = _hp;
        _hp -= damage;
        HpChangedEventArgs hpChangedEventArgs = new HpChangedEventArgs();
        hpChangedEventArgs.OldHp = OldHp;
        hpChangedEventArgs.NewHp = _hp;
        hpChangedEventArgs.Damage = damage;
        
        HpChanged?.Invoke(this, hpChangedEventArgs);
    }
}

class HpChangedEventArgs : EventArgs
{
    public int OldHp { get; set; }
    public int NewHp { get; set; }
    public int Damage { get; set; }
}