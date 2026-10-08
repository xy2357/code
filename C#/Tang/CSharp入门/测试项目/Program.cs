namespace 测试项目
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 0;
            int b = 0;
            while (a < 10)
            {
                ++a;
                if (b < 10)
                {
                    ++b;
                }
            }
            Console.WriteLine(a);
            Console.WriteLine(b);
        }
    }
}
