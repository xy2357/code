namespace Question24;

class Program
{
    static void Main(string[] args)
    {
        Monster monster = new Monster("史莱姆", 100);
        monster.Died += OnMonsterDied;
        monster.TakeDamage(20);
        monster.TakeDamage(50);
        monster.TakeDamage(60);
    }

    static void OnMonsterDied(object? sender, MonsterDiedEventArgs e)
    {
        Console.WriteLine($"{e.MonsterName}死亡，掉落金币{e.Gold}");
    }
}

class Monster
{
    private string _name;
    private int _hp;
    private bool _isDead = false;

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
            e.Gold = 20;
            Died?.Invoke(this,e);
        }
    }
}

class MonsterDiedEventArgs: EventArgs
{
    public string MonsterName { get; set; } = "";
    public int Gold{ get; set; }
}