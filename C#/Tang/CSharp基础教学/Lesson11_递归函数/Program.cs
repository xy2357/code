namespace Lesson11_递归函数
{
    internal class Program
    {
        static void Fun(int a)
        {
            Console.WriteLine(a);
            ++a;
            if (a > 10) 
            {
                return;
            }
            Fun(a);
        }
        static void Main(string[] args)
        {
            Fun(0);
        }
    }
}
