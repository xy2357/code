namespace Lesson13_冒泡排序
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] nums = new int[] { 22, 31, 65, 34, 36, 134, 76, 32, 12, 4, 32 };
            bool isScort;
            for (int i = 0; i < nums.Length - 1; i++)
            {
                isScort = false;
                for (int j = 0; j < nums.Length - 1 - i; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        isScort = true;
                        int k = nums[j + 1];
                        nums[j + 1] = nums[j];
                        nums[j] = k;
                    }
                }
                if (!isScort)
                {
                    break;
                }
            }
            foreach (var num in nums)
            {
                Console.WriteLine(num);
            }
        }
    }
}
