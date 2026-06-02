namespace Question2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hero h1 = new Hero("亚瑟", 100, 30);

            Hero h2 = new Hero("盖伦", 100, 20);

            h1.Attack(h2);
        }
    }

    class Hero
    {
        private string _name;
        private int _hp;
        private int _atk;

        public Hero(string name, int hp, int atk)
        {
            _name = name;
            _hp = hp;
            _atk = atk;
        }

        public void Attack(Hero hero)
        {
            Console.WriteLine($"{this._name}攻击了{hero._name},造成了{this._atk}点伤害");
            Console.WriteLine($"{hero._name}剩余血量：{(hero._hp) - (this._atk)}");
        }
    }
}

