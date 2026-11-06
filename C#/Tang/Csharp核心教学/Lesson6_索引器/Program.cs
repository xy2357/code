namespace Lesson6_索引器
{
    internal class Program
    {
        class Person
        {
            public string name;
            public int age;
            private Person[] friends;
            private int[,] array;

            public int this[int i, int j]
            {
                get
                {
                    return array[i, j];
                }
                set
                {
                    array[i, j] = value;
                }
            }

            public string this[string str]
            {
                get
                {
                    switch (str)
                    {
                        case "name":
                            return this.name;
                        case "age":
                            return this.age.ToString();
                    }
                    return "";
                }
            }

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
            Console.WriteLine(p[0]);

            p[0, 0] = 10;

        }
    }
}
