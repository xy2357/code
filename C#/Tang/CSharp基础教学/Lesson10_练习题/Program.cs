namespace Lesson10_练习题
{
    internal class Program
    {
        static float GetMax(int a, float b)
        {
            if ((float)a > b)
            {
                return a;
            }
            return b;
        }

        static double GetMax(double a, double b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        static int Compare(params int[] arr)
        {
            int max = arr[0]; 
            foreach (var num in arr)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }
        static float Compare(params float[] arr)
        {
            float max = arr[0];
            foreach (var num in arr)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }
        static double Compare(params double[] arr)
        {
            double max = arr[0];
            foreach (var num in arr)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }

        static void Main(string[] args)
        {
            GetMax(1, 1.2);
            GetMax(1, 1.2f);
            Compare(1, 2, 3, 4);
            Compare(1.0f, 2, 3, 4);
            Compare(1.1, 2, 3, 4);
        }
    }
}
