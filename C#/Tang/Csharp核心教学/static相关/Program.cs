namespace static相关
{
    class Test
    {
        public static int a = 100;
        static Test()
        {
            Console.WriteLine("静态构造函数");
        }

        public Test()
        {
            Console.WriteLine("普通构造函数");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Test t = new Test();
        }
    }
}
