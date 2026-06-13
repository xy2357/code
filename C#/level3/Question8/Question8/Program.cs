using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            player.HpChanged += player.OnHpChanged;
            player.TakeDamage(20);
            player.TakeDamage(30);
        }
    }

    class Player
    {
        int hp = 100;
        public event Action<int> HpChanged;

        public void TakeDamage(int damage)
        {
            hp -= damage;
            this.HpChanged?.Invoke(hp);
        }

        internal void OnHpChanged(int hp)
        {
            Console.WriteLine($"英雄血量为：{hp}");
        }
    }
}
