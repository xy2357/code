namespace Question19;

class Program
{
    static void Main(string[] args)
    {
        Player player = new Player("xy", 100);
        player.HpChanged += player.OnHpChanged;
        player.TakeDamage(10);
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

    public event EventHandler<HpChangedEventArgs> HpChanged;

    public void TakeDamage(int damage)
    {
        int oldHp = _hp;
        _hp -= damage;

        HpChangedEventArgs hpChangedEventArgs = new HpChangedEventArgs();
        hpChangedEventArgs.OldHp = oldHp;
        hpChangedEventArgs.NewHp = _hp;
        hpChangedEventArgs.Damage = damage;
        
        HpChanged?.Invoke(this,hpChangedEventArgs);
    }
    
    public void OnHpChanged(object? sender, HpChangedEventArgs e)
    {
        Console.WriteLine($"旧血量：{e.OldHp}");
        Console.WriteLine($"新血量：{e.NewHp}");
        Console.WriteLine($"伤害：{e.Damage}");
    }
}

class HpChangedEventArgs : EventArgs
{
    public int OldHp {get; set; }
    public int NewHp { get; set; }
    public int Damage { get; set; }
}