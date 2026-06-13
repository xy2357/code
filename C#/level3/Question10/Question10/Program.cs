using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player("亚索", 100);
            Monster monster = new Monster("小兵", 100);
            player.Died += player.OnDied;
            monster.Died += monster.OnDied;

            Attack(player, 20);
            Attack(player, 50);
            Attack(player, 90);

            Attack(monster, 20);
            Attack(monster, 50);
            Attack(monster, 90);

        }

        static void Attack(IDamageable target, int damage)
        {
            if (target == null) { return; }
            target.TakeDamage(damage);
        }
    }

    interface IDamageable
    {
        void TakeDamage(int damage);
    }

    class Player : IDamageable
    {
        private string _name;
        private int _hp;
        private bool is_Dead = false;

        public Player(string name,int hp)
        {
            _name = name;
            _hp = hp;
        }

        public event Action Died;
        public void TakeDamage(int damage)
        {
            if (is_Dead) { return; }

            _hp -= damage;
            Console.WriteLine($"{_name}受到{damage},剩余血量为{_hp}");

            if (_hp <= 0)
            {
                is_Dead = true;
                this.Died?.Invoke();
            }
        }

        public void OnDied()
        {
            Console.WriteLine("英雄死亡");
        }
    }

    class Monster : IDamageable
    {
        private string _name;
        private int _hp;
        private bool is_Dead = false;

        public Monster(string name, int hp)
        {
            _name = name;
            _hp = hp;
        }

        public event Action Died;
        public void TakeDamage(int damage)
        {
            if (is_Dead) { return; }

            _hp -= damage;
            Console.WriteLine($"{_name}受到{damage},剩余血量为{_hp}");

            if (_hp <= 0)
            {
                is_Dead = true;
                this.Died?.Invoke();
            }
        }

        public void OnDied()
        {
            Console.WriteLine("怪物死亡，掉落金币");
        }
    }
}
