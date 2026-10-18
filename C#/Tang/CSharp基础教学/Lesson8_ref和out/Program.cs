namespace Lesson8_ref和out
{
    internal class Program
    {

        static void ChangeValue(int value)
        {
            value = 3;
        }

        //static void ChangeArrayValue(int[] arr)
        //{
        //    arr[0] = 99;
        //}

        //static void ChangeValueRef(ref int value)
        //{
        //    value = 3;
        //}

        static void ChangeArrayRef(ref int[] arr)
        {
            arr = new int[] { 10, 20, 30 };
        }

        static void ChangeValueOut(out int value)
        {
            value = 3;
        }

        static void ChangeArrayOut(out int[] arr)
        {
            arr = new int[] { 10, 20, 30 };
        }
        static void Main(string[] args)
        {
            //int a = 1;
            //ChangeValue(a);
            //Console.WriteLine(a);

            //int[] arr2 = { 1, 2, 3 };
            //ChangeArrayValue(arr2);
            //Console.WriteLine(arr2[0]);

            //int a = 1;
            //ChangeValueRef(ref a);
            //Console.WriteLine(a);

            //int[] arr2 = { 1, 2, 3 };
            //ChangeArrayRef(ref arr2);
            //Console.WriteLine(arr2[0]);

            //int a;
            //ChangeValueOut(out a);
            //Console.WriteLine(a);

            int[] arr2 = { 1, 2, 3 };
            ChangeArrayOut(out arr2);
            Console.WriteLine(arr2[0]);

        }
    }
}
