using System.Globalization;

namespace Qustion17;

class Program
{
    static void Main(string[] args)
    {
        Player player = new Player();
        player.HpChanged += hp =>
        {
            Console.WriteLine($"当前血量 ：{hp}");
        };
        player.TakeDamage(10);
    }
}

class Player
{
    private int _hp = 100;

    public event Action<int>? HpChanged;

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        HpChanged?.Invoke(_hp);
    }
}