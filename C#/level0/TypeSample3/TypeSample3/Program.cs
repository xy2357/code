using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeSample3
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            Type myType = typeof(Type);
            PropertyInfo[] pInfos= myType.GetProperties();
            MethodInfo[] mInfos = myType.GetMethods();
            foreach (var p in pInfos)
            {
                Console.WriteLine(p.Name);
            }
        }
    }
}
