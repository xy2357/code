
namespace Question13;

class Program
{
    static void Main(string[] args)
    {
        Player player = new Player("xiaowang");
        player.HpChanged += player.OnHpChanged;
        player.TakeDamage(10);
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
    
    public event Action<int> HpChanged;

    public void TakeDamage(int damage)
    {
        if (_hp <= 0)
        {
            return;
        }

        _hp -= damage;
        Console.WriteLine($"{_name}受到伤害{damage}");
        HpChanged?.Invoke(_hp);
    }
    
    public void OnHpChanged(int hp)
    {
        Console.WriteLine($"当前血量：{hp}");
    }
}