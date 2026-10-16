namespace 特殊引用类型
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string str1 = "123";
            //string str2 = str1;
            //str2 = "321";

            int[] a = new int[] { 10 };
            int[] b = a;
            b = new int[5];
            Console.Write(a[0]);
        }
    }
}
