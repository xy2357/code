namespace Question6
{

    delegate int Calc(int a, int b);

    class Program
    {
        static void Main(string[] args)
        {
            DoCalc(Calculator.Add, 10, 20);
            DoCalc(Calculator.Mul, 10, 20);
            Console.ReadKey();
        }

        static void DoCalc(Calc calc,int x, int y)
        {
            Console.WriteLine(calc(x, y));
        }
    }

    class Calculator
    {
        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static int Mul(int a, int b)
        {
            return a * b;
        }
    }

}

