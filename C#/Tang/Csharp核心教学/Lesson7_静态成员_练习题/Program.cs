namespace Lesson7_静态成员_练习题
{
    class Test
    {
        private static Test t = new Test();

        public int testInt = 10;

        public static Test T
        {
            get
            {
                return t;
            }
        }

        private Test()
        {

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Test.T.testInt);
        }
    }
}
