namespace Lesson11_继承
{
    class Person
    {
        private string _name;
        private int _age;

        public Person(string name,int age)
        {
            Name = name;
            Age = age;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public void Speak()
        {
            Console.WriteLine("我会说话！");
        }
    }

    class Warrior : Person
    {
        private int _atk;
        public Warrior(string name , int age, int atk) : base(name,age)
        {
            Atk = atk;
        }
        
        public int Atk
        {
            get
            {
                return _atk;
            }
            set
            {
                _atk = value;
            }
        }

        public void Attack()
        {
            Console.WriteLine("我会攻击！");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Person p1 = new Person("小李", 20);
            Warrior w1 = new Warrior("小王", 30, 13);
        }
    }
}
