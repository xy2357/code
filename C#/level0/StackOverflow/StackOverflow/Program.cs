using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //unsafe
            //{
            //    int* p = stackalloc int[9999999];
            //}
            BadGay bg = new BadGay();
            bg.BadMethod();
        }
    }

    class BadGay
    {
        public void BadMethod()
        {
            int x = 100;
            this.BadMethod();
        }
    }
}
