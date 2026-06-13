using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Question9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Monster monster = new Monster("史莱姆",100);
            monster.Died += monster.OnDied;
            monster.TakeDamage(50);
            monster.TakeDamage(40);
            monster.TakeDamage(20);
        }
    }

    class Monster
    {
        private string _name;
        private int _hp;
        private bool _isDead = false;

        public Monster(string name,int hp)
        {
            _name = name;
            _hp = hp;
        }

        public event Action Died;


        public void TakeDamage(int damage)
        {
            if (_isDead)
            {
                Console.WriteLine($"{_name}已经死亡，不能再收到伤害");
                return;
            }

            _hp -= damage;
            Console.WriteLine($"{_name}收到{damage},剩余血量{_hp}");

            if (_hp <= 0)
            {
                _isDead = true;
                this.Died?.Invoke();
            }
        }

        public void OnDied()
        {
            Console.WriteLine("怪物死亡，掉落金币！");
        }

    }
}
