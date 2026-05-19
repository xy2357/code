using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Hanoi(int n, string from, string to, string help)
        {
            if(n==1)
            {
                //只剩一个盘子，直接移动
                Console.WriteLine($"{from} -> {to}");
                return;
            }
            else
            {
                //把上面n-1个盘子从from移动到help上,可以借助to
                Hanoi(n - 1, from, help, to);

                //把最大的盘子从from移动到to
                Console.WriteLine($"{from} -> {to}");

                //把help上的n-1个盘子移动到to，可以借助from
                Hanoi(n - 1, help, to, from);

            }
        }

        static void Main()
        {
            Hanoi(4, "A", "C", "B");
            Console.ReadKey();
        }
    }
}