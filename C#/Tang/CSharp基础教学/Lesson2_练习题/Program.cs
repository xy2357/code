namespace Lesson2_练习题
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 练习一
            //int[] nums = new int[100];
            //for (int i = 0; i < nums.Length; i++)
            //{
            //    nums[i] = i;
            //}
            //foreach (var num in nums)
            //{
            //    Console.WriteLine(num);
            //}
            #endregion

            #region 练习二
            //int[] array1 = { 1, 2, 3, 4, 5 };
            //int[] array2 = new int[5];
            //for (int i = 0;i < array1.Length; i++)
            //{
            //    array2[i] = array1[i] * 2;
            //}
            //foreach (var num in array2)
            //{
            //    Console.WriteLine(num);
            //}
            #endregion

            #region 练习三
            //int[] array3 = new int[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    Random r = new Random();
            //    array3[i] = r.Next(0, 101);
            //}
            //foreach(var num in array3)
            //{
            //    Console.WriteLine(num);
            //}
            #endregion

            #region 练习四
            //int[] array4 = { 3, 32, 54, 67, 1, 99, 0 };
            //int max = 0;
            //int min = 0;
            //int sum = 0;
            //int average = 0;
            //foreach (var num in array4)
            //{
            //    if (num > max)
            //    {
            //        max = num;
            //    }
            //    else if (num < min)
            //    {
            //        min = num;
            //    }
            //    sum += num;
            //}
            //average = sum / array4.Length;

            //Console.WriteLine(max);
            //Console.WriteLine(min);
            //Console.WriteLine(sum);
            //Console.WriteLine(average);
            #endregion

            #region 练习五
            //int[] array5 = { 1, 2, 3, 4, 5, 6 };
            //int[] array5_copy = (int[])array5.Clone();
            //for (int i = 0; i < array5.Length; i++)
            //{
            //    array5_copy[i] = array5[array5.Length - 1 - i];
            //}
            //foreach (var num in array5_copy)
            //{
            //    Console.WriteLine(num);
            //}
            #endregion

            #region 练习六
            //int[] array6 = { -2, 3, 6, -9, 0, 7 };
            //for (int i = 0; i < array6.Length; i++)
            //{
            //    if (array6[i] < 0)
            //    {
            //        array6[i] += 1;
            //    }
            //    else if (array6[i] > 0)
            //    {
            //        array6[i] -= 1;
            //    }
            //}
            //foreach (int num in array6)
            //{
            //    Console.WriteLine(num);
            //}

            #endregion

            #region 练习七
            //int[] array7 = new int[10];
            //for (int i = 0; i < array7.Length; i++)
            //{
            //    Console.WriteLine("请输入成绩：");
            //    array7[i] = int.Parse(Console.ReadLine());
            //}
            //foreach (var num in array7)
            //{
            //    Console.WriteLine(num);
            //}
            #endregion

            #region 练习八
            string[] iconArray = new string[25] { "□","■","□","■","□","■", "□","■", "□","■" ,
                "□","■", "□","■", "□","■", "□","■" ,"□","■" ,
                "□","■","□","■","□"};
            for (int i = 0; i < iconArray.Length; i++)
            {
                if ((i + 1) % 5 == 0)
                {
                    Console.WriteLine(iconArray[i]);
                }
                else 
                {
                    Console.Write(iconArray[i]);
                }
            }

            #endregion
        }
    }
}
