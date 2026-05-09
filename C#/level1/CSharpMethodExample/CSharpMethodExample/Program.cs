namespace CSharpMethodExample
{
    class Program
    {
        static void Main(string[] args)
        {
            double result = Calculator.GetConeVolumns(100, 100);
        }
    }

    class Calculator
    {
        public static double GetCircleArea(double r)
        {
            return Math.PI * r;
        }

        public static double GetCylinderVolumn(double r, double h)
        {
            double a = GetCircleArea(r);
            return a * h;
        }

        public static double GetConeVolumns(double r, double h)
        {
            double cv = GetCylinderVolumn(r, h);
            return cv * h;
        }
    }
}
