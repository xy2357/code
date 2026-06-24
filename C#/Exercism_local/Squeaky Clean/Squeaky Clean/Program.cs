using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squeaky_Clean
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string result = Identifier.Clean("My😀😀Finder😀");
            Console.WriteLine(result);
        }
    }

    public static class Identifier
    {
        public static string Clean(string identifier)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < identifier.Length; i++)
            {
                char currentChar = identifier[i];

                //下划线替换成空格
                if (char.IsWhiteSpace(currentChar))
                {
                    sb.Append('_');
                }
                //控制字符替换成 CTRL
                else if (char.IsControl(currentChar))
                {
                    sb.Append("CTRL");
                }
                //遇到短横线，后面一个字符转大写
                else if (currentChar == '-')
                {
                    if (i + 1 < identifier.Length)
                    {
                        sb.Append(char.ToUpper(identifier[i + 1]));
                        i++;
                    }
                }
                //保留字母和下划线
                //排除小写希腊字母
                else if
                    (
                    (Char.IsLetter(currentChar)) && ((currentChar < 'α') || (currentChar > 'ω'))
                    || currentChar == '_'
                    )
                {
                    sb.Append(currentChar);
                }
            }
            return sb.ToString();
        }
    }
}
