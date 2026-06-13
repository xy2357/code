using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Question12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player("小李");
            player.HpChanged += OnHpChanged;
            player.TakeDamage(30);
            player.TakeDamage(40);
            player.TakeDamage(50);
        }

        static void OnHpChanged(int hp)
        {
            Console.WriteLine($"当前血量：{hp}");
        }
    }

    class Player
    {
        private string _name;
        private int _hp = 100;
        private bool is_Dead = false;

        public Player(string name)
        {
            _name = name;
        }

        public event Action<int> HpChanged;

        public void TakeDamage(int damage)
        {
            _hp -= damage;
            this.HpChanged?.Invoke(_hp);
            Console.WriteLine($"{_name}受到{damage}伤害");
        }
    }
}
