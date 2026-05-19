namespace isasSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //object o = new Teacher();
            //Teacher t = o as Teacher;
            //if (t != null)
            //{
            //    t.Teach();
            //}

            object o = new Teacher();
            Console.WriteLine(o.GetType().Name);
            if(o is Teacher)
            {
                Teacher t = (Teacher)o;
                t.Teach();
            }
        }
    }

    class Animal
    {
        public void Eat()
        {
            Console.WriteLine("I like eatting!");
        }
    }

    class Human:Animal
    {
        public void Think()
        {
            Console.WriteLine("Who I am?");
        }
    }

    class Teacher:Human
    {
        public void Teach()
        {
            Console.WriteLine("I teach Programming!");
        }
    }

    class Car
    {
        public void Speed()
        {
            Console.WriteLine("Go! Go! Go!");
        }
    }
}
