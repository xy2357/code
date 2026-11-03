namespace Lesson6_索引器
{
    internal class Program
    {
        class Person
        {
            public string name;
            public int age;
            private Person[] friends;

            public Person this[int index]
            {
                get
                {
                    return friends[index];
                }
                set
                {
                    friends[index] = value;
                }
            }
        }

        static void Main(string[] args)
        {
            Person p = new Person();
            p[0] = new Person();
            p[0].name;
        }
    }
}
