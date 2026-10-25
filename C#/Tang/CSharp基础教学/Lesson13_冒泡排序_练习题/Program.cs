namespace Lesson13_冒泡排序_练习题
{
    internal class Program
    {
        #region 升序
        static int[] AscendSort(int[] arr)
        {
            bool isSort;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                isSort = false;
                for (int j = 0; j < arr.Length - 1 - i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        isSort = true;
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
                if (!isSort)
                {
                    break;
                }
            }

            return arr;
        }
        #endregion

        #region 降序
        public static int[] DescendSort(int[] arr)
        {
            bool isSort;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                isSort = false;
                for (int j = 0; j < arr.Length - 1 - i; j++)
                {
                    if (arr[j] < arr[j + 1])
                    {
                        isSort = true;
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
                if (!isSort)
                {
                    break;
                }
            }

            return arr;
        }
        #endregion
        static int[] Sort(int i, int[] arr)
        {
            if (i == 0)
            {
                DescendSort(arr);
            }
            else
            {
                AscendSort(arr);
            }
            return arr;
        }
        static void Main(string[] args)
        {
            int[] arr = new int[20];
            Random r = new Random();
            bool isSort;
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = r.Next(0,101);
            }
            foreach (var num in arr)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();

            Console.WriteLine("************************");

            foreach (var num in Sort(1, arr))
            {
                Console.Write(num + " ");
            }
        }
    }
}
