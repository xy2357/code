namespace Question15;

class Program
{
    static void Main(string[] args)
    {
        Monster monster = new Monster("xy",100);
        monster.Died += OnMonsterDied;
        monster.TakeDamage(40);
        monster.TakeDamage(70);
        monster.TakeDamage(70);
    }

    static void OnMonsterDied()
    {
        Console.WriteLine("怪物死亡！");
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

    public event Action? Died;

    public void TakeDamage(int damage)
    {
        if (_isDead)
        {
            Console.WriteLine("怪物已死亡！不能再受到伤害！");
            return;
        }
        
        _hp -= damage;
        Console.WriteLine($"{_name}受到{damage}伤害");


        if (_hp <= 0)
        {
            _isDead = true;
            Died?.Invoke();
            return;
        }
    }
}