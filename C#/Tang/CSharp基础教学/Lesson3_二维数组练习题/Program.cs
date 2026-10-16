namespace Lesson3_二维数组练习题
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 练习一
            //int[,] array1 = new int[100, 100];
            //int index = 0;
            //for (int i = 0; i < array1.GetLength(0); i++)
            //{
            //    for (int j = 0; j < array1.GetLength(1); j++)
            //    {
            //        array1[i, j] = index;
            //        index ++;
            //    }
            //}

            //foreach (var num in array1)
            //{
            //    Console.WriteLine(num);
            //}
            #endregion

            #region 练习二
            int[,] array2 = new int[4, 4];
            Random r = new Random();
            for (int i = 0; i < array2.GetLength(0); i++)
            {
                for (int j = 0; j < array2.GetLength(1); j++)
                {
                    array2[i, j] = r.Next(1, 101);
                    if (i > 1 && j <=1 )
                    {
                        array2[i, j] = 0;
                    }
                }
            }

            foreach (var item in array2)
            {
                Console.WriteLine(item);
            }
            #endregion

            #region 练习三
            //int[,] array3 = new int[3, 3];
            //Random r = new Random();
            //int sum1 = 0;
            //int sum2 = 0;
            //for (int i = 0; i < array3.GetLength(0); i++)
            //{
            //    for (int j = 0; j < array3.GetLength(1); j++)
            //    {
            //        array3[i, j] = r.Next(1, 11);
            //    }
            //}

            //foreach (var item in array3)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine("****************************");

            //for (int i = 0; i < 3; i++)
            //{
            //    sum1 += array3[i, i];
            //}
            //Console.WriteLine(sum1);

            //for (int i = 0; i < 3; i++)
            //{
            //    sum2 += array3[i, 3 - i - 1];
            //}
            //Console.WriteLine(sum2);
            #endregion

            #region 练习四
            //int[,] array4 = new int[4, 4];
            //Random r = new Random();
            //int max = 0;
            //int maxI = 0;
            //int maxJ = 0;
            //for (int i = 0; i < array4.GetLength(0); i++)
            //{
            //    for (int j = 0; j < array4.GetLength(1); j++)
            //    {
            //        array4[i, j] = r.Next(1, 501);
            //        if (array4[i, j] > max)
            //        {
            //            max = array4[i, j];
            //            maxI = i;
            //            maxJ = j;
            //        }
            //    }
            //}

            //foreach (var item in array4)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine("***********************");

            //Console.WriteLine("最大值是{0}，在{1}行{2}列", max, maxI, maxJ);
            #endregion

            #region 练习五
            //int[,] array5 = new int[5, 5] { {0,0,0,0,0},
            //                                {0,0,0,0,0},
            //                                {0,1,0,0,0},
            //                                {0,0,1,1,0},
            //                                {0,0,0,0,0}, };

            //int[,] array6 = (int[,])array5.Clone();
            //for (int i = 0; i < array5.GetLength(0); i++)
            //{
            //    for (int j = 0; j < array5.GetLength(1); j++)
            //    {
            //        if (array5[i, j] == 1)
            //        {
            //            for (int x = 0; x < array6.GetLength(0); x++)
            //            {
            //                array6[x, j] = 1;
            //            }
            //            for (int y = 0; y < array6.GetLength(1); y++)
            //            {
            //                array6[i, y] = 1;
            //            }
            //        }
            //    }
            //}

            //foreach (var item in array6)
            //{
            //    Console.WriteLine(item);
            //}

            #endregion
        }
    }
}
