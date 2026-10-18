namespace Lesson10_函数重载
{
    internal class Program
    {
        static float Calc(ref float f, int a)
        {
            return f + a;
        }
        static float Calc(out float f, int a)
        {
            f = 1;
            return f + a;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
