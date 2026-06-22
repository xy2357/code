using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squeaky_Clean
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string result = Identifier.Clean("my\0Id");
            Console.WriteLine(result);
        }
    }

    public static class Identifier
    {
        public static string Clean(string identifier)
        {
            const string str = "CTRL";
            identifier = identifier.Replace(" ", "_");
            //identifier = identifier.Replace("\\", str);
            return identifier;
        }
    }
}
