namespace Question14;

class Program
{
    static void Main(string[] args)
    {
        Player player = new Player("xy", 100);
        player.Damaged += player.OnDamaged;
        player.TakeDamage(10);
        player.TakeDamage(30);
    }
}

class Player
{
    private string _name;
    private int _hp = 100;

    public Player(string name, int hp)
    {
        _name = name;
        _hp = hp;
    }

    public event Action<int>? Damaged;

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        Damaged?.Invoke(damage);
        Console.WriteLine($"{_name}剩余血量：{_hp} ");
    }

    public void OnDamaged(int damage)
    {
        Console.WriteLine($"玩家受到了{damage}伤害");
    }
}