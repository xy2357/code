using System.Timers;

namespace DelegateExample
{

    public delegate double Calc(double x, double y);

    internal class Program
    {
        static void Main(string[] args)
        {
            double a = 100;
            double b = 200;
            double c = 0;

            Calculator calculator = new Calculator();
            c = calculator.Add(a, b);

            Calc calc1 = new Calc(calculator.Add);
            Calc calc2 = new Calc(calculator.Sub);
            Calc calc3 = new Calc(calculator.Mul);
            Calc calc4 = new Calc(calculator.Div);

            c = calc1(a, b);
            Console.WriteLine(c);

        }
    }

    class Calculator
    {
        public double Add(double x, double y)
        {
            return x + y;
        }
        public double Sub(double x, double y)
        {
            return x - y;
        }
        public double Mul(double x, double y)
        {
            return x * y;
        }
        public double Div(double x, double y)
        {
            return x / y;
        }
    }
}
