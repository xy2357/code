using System.Formats.Asn1;

namespace AfkBattlePrototype
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Character hero1 = new Character("hero1", 100, 20, 10);
            Character hero2 = new Character("hero2", 80, 15, 12);
            hero1.TakeDamage(hero2);
            Console.WriteLine(hero2.Hp);
        }
    }

    class Character
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Character(string name, int hp, int attack, int defense)
        {
            Name = name;
            Hp = hp;
            Attack = attack;
            Defense = defense;
        }

        public void TakeDamage(Character target)
        {
            target.Hp -= Attack;
        }

        public bool IsDead()
        {
            if (this.Hp < 0) return true;
            return false;
        }
    }
}
