namespace Lesson14_选择排序_练习题
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 升序
            //int[] arr = new int[] { 2, 3, 9, 5, 7, 6, 8, 4, 1 };
            //for (int i = 0; i < arr.Length - 1; i++)
            //{
            //    int index = 0;
            //    for (int j = 1; j < arr.Length - i; j++)
            //    {
            //        if (arr[index] < arr[j])
            //        {
            //            index = j;
            //        }
            //    }
            //    if (index != arr.Length - 1 - i)
            //    {
            //        int temp = arr[index];
            //        arr[index] = arr[arr.Length - 1 - i];
            //        arr[arr.Length - 1 - i] = temp;
            //    }
            //}
            //foreach (var num in arr)
            //{
            //    Console.WriteLine(num);
            //}
            #endregion

            #region 降序
            int[] arr = new int[] { 2, 3, 9, 5, 7, 6, 8, 4, 1 };
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int index = 0;
                for (int j = 1; j < arr.Length - i; j++)
                {
                    if (arr[index] >  arr[j])
                    {
                        index = j;
                    }
                }
                if (index != arr.Length - 1 - i)
                {
                    int temp = arr[index];
                    arr[index] = arr[arr.Length - 1 - i];
                    arr[arr.Length - 1 - i] = temp;
                }
            }
            foreach (var num in arr)
            {
                Console.WriteLine(num);
            }
            #endregion
        }
    }
}
