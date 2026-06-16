namespace Question25;

class Program
{
    static void Main(string[] args)
    {
        Monster monster = new Monster("史莱姆", 100);
        monster.Died += DropGold;
        monster.Died += AddExp;
        monster.Died += PlayDeathSound;
        monster.TakeDamage(20);
        monster.TakeDamage(50);
        monster.TakeDamage(60);
    }

    static void DropGold(object? sender, MonsterDiedEventArgs e)
    {
        Console.WriteLine($"{e.MonsterName}死亡，掉落金币{e.Gold}");
    }    
    
    static void AddExp(object? sender, MonsterDiedEventArgs e)
    {
        Console.WriteLine($"{e.MonsterName}死亡，掉落经验{e.Exp}");
    }
    
    static void PlayDeathSound(object? sender, MonsterDiedEventArgs e)
    {
        Console.WriteLine($"{e.MonsterName}死亡，播放音乐！");
    }
}

class Monster
{
    private string _name;
    private int _hp;
    private bool _isDead = false;
    private int _gold = 20;
    private int _exp = 10;

    public Monster(string name, int hp)
    {
        _name = name;
        _hp = hp;
    }

    public event EventHandler<MonsterDiedEventArgs>? Died;

    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        _hp -= damage;
        if (_hp <= 0)
        {
            _isDead = true;
            MonsterDiedEventArgs e = new MonsterDiedEventArgs();
            e.MonsterName = _name;
            e.Gold = _gold;
            e.Exp = _exp;
            Died?.Invoke(this,e);
        }
    }
}

class MonsterDiedEventArgs: EventArgs
{
    public string MonsterName { get; set; } = "";
    public int Gold{ get; set; }
    public int Exp { get; set; }
}