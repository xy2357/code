namespace Question6
{

    delegate int Calc(int a, int b);

    class Program
    {
        static void Main(string[] args)
        {
            Calc c1 = new Calc(Calculator.Add);
            Calc c2 = new Calc(Calculator.Mul);

            Console.WriteLine(c1(10, 20));
            Console.WriteLine(c2(10, 20));
        }

        static void DoCalc(Calc calc,int x, int y)
        {
            
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

