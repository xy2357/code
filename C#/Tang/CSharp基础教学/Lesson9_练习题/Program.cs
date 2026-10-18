namespace Lesson9_练习题
{
    internal class Program
    {

        static int Average(params int[] array)
        {
            if (array.Length == 0)
            {
                throw new ArgumentException("请输入数字", "array");
            }
            int sum = 0;
            foreach (var num in array)
            {
                sum += num;
            }
            return sum / array.Length;
        }

        static void OddAndEvenSum(params int[] array)
        {
            int evenSum = 0;
            int oddSum = 0;
            if (array.Length == 0)
            {
                return;
            }
            foreach (var num in array)
            {
                if (num % 2 == 0)
                {
                    evenSum += num;
                }
                else
                {
                    oddSum += num;
                }
            }
            Console.WriteLine("偶数和是：{0}，奇数和是：{1}", evenSum, oddSum);
        }
        static void Main(string[] args)
        {
            //try
            //{
            //    Console.WriteLine(Average());
            //}
            //catch (ArgumentException)
            //{
            //    Console.WriteLine("请输入数字");
            //}

            OddAndEvenSum();
            Console.WriteLine(1);
        }
    }
}
