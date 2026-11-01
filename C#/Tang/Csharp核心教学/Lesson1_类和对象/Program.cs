  namespace Lesson1_类和对象
{
    class Person
    {
        public string name;
        public int age;
        public bool sex;

        public Person(string name)
        {
            this.name = name;
        }
        public Person(int age)
        {
            this.age = age;
        }
        public Person(string name, int age)
        {
            this.name = name;
            this.age = age + 1;
        }

        public Person(string name, int age, bool sex):this(name)
        {
            this.name = name;
            this.age = age;
            this.sex = sex;
        }
        
        ~Person()
        {

        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person(1);
            Console.WriteLine(GC.GetGeneration(p));   // 0，刚创建

            GC.Collect();
            Console.WriteLine(GC.GetGeneration(p));   // 1，活过一次 GC

            GC.Collect();
            Console.WriteLine(GC.GetGeneration(p));   // 2，活过两次 GC
        }
    }
}
