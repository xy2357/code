namespace Lesson2_数组
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = array1.Append(1).ToArray();
            foreach (var num in array2)
            {
                Console.WriteLine(num);
            }
        }
    }
}
