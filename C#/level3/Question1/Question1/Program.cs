namespace Question1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hero h1 = new Hero();
            h1.Name = "小狗";
            h1.Hp = 100;
            h1.Atk = 30;

            Hero h2 = new Hero();
            h2.Name = "盖伦";
            h2.Hp = 100;
            h2.Atk = 20;

            h1.Attack(h2);
        }
    }

    class Hero
    {
        public string Name {  get; set; }
        public int Hp {  get; set; }
        public int Atk {  get; set; }

        public void Attack(Hero hero)
        {
            Console.WriteLine($"{this.Name}攻击了{hero.Name},造成了{this.Atk}点伤害");
            Console.WriteLine($"{hero.Name}剩余血量：{(hero.Hp)-(this.Atk)}");
        }
    }
}
