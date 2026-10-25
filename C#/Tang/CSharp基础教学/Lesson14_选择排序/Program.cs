namespace Lesson14_选择排序
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[] {23,54,77,33,12,21,75,98,3};
            for (int j = 0; j < arr.Length; j++)
            {
                int index = 0;
                for (int i = 1; i < arr.Length - j; i++)
                {
                    if (arr[index] < arr[i])
                    {
                        index = i;
                    }
                }
                if (index != arr.Length - 1 - j)
                {
                    int temp = arr[index];
                    arr[index] = arr[arr.Length - 1 - j];
                    arr[arr.Length - 1 - j] = temp;
                }
            }
            foreach (var num in arr)
            {
                Console.WriteLine(num);
            }
        }
    }
}
