namespace Lesson11_练习题
{
    internal class Program
    {
        #region 练习一
        static void Func1(int a)
        {
            Console.WriteLine(a);
            --a;
            if (a < 0)
            {
                return;
            }
            Func1(a);
        }
        #endregion

        #region 练习二
        static int Func2(int a)
        {
            if (a <= 1)
            {
                return 1;
            }
            return a * Func2(a - 1);
        }
        #endregion


        #region 练习三
        static int Func3(int a)
        {
            if (a <= 1)
            {
                return 1;
            }
            return Func2(a) + Func3(a - 1);
        }
        #endregion

        #region 练习四
        static float Func4(int a)
        {
            if (a <= 0)
            {
                return 100;
            }
            return (float)Func4(a - 1) / 2;
        }
        #endregion

        #region 练习五
        static bool Func5(int a)
        {
            Console.WriteLine(a);
            return a == 200 || Func5(a + 1);
        }
        #endregion

        #region 练习六
        static int Func6(int a)
        {
            if (a <= 1)
            {
                return 1;
            }
            return a + Func6(a - 1);
        }
        #endregion

        #region 练习七
        static int Func7(int x,int n)
        {
            if (n == 0)
            {
                return 1;
            }
            return x * Func7(x, n - 1);
        }
        #endregion

        #region 练习八
        static int Func8(int a)
        {
            if (a <=2 )
            {
                return 1;
            }
            return Func8(a - 1) + Func8(a - 2);
        }
        #endregion

        #region 练习九
        static int Func9(int a)
        {
            if (a / 10 < 1)
            {
                return a % 10;
            }
            return a % 10 + Func9(a / 10);
        }
        #endregion

        #region 练习十
        static int Func10(int a,int b)
        {
            if (b == 0)
            {
                return a;
            }
            return Func10(b, a % b);
        }
        #endregion

        #region 练习十一
        static string Func11(string s)
        {
            if (s.Length <= 1)
            {
                return "是回文";
            }
            return s.Substring(1, s.Length - 2);
        }
        #endregion
        static void Main(string[] args)
        {
            //Func1(10);
            //Console.WriteLine(Func2(5));
            //Console.WriteLine(Func3(10));
            //Console.WriteLine(Func4(10));
            //Func5(1);
            //Console.WriteLine(Func6(100));
            //Console.WriteLine(Func7(2, 10));
            //Console.WriteLine(Func8(3));
            //Console.WriteLine(Func9(1234));
            //Console.WriteLine(Func10(48,18));
            Func11("level");
        }
    }
}
