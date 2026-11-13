namespace Lesson8_静态类和静态构造函数
{
    static class Tool
    {
        public static float CalCircleArea(float r)

        {
            return 3.14f * r * r;
        }

        public static float CalCirclePerimeter(float r)
        {
            return 3.14f * 2f * r;
        }
        public static float CalRectangleArea(float h,float w)

        {
            return h * w;
        }

        public static float CalRectanglePerimeter(float h, float w)
        {
            return (h + w) * 2;
        }

        public static float CalAbsolute(float i)
        {
            return Math.Abs(i);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Tool.CalCircleArea(3));
            Console.WriteLine(Tool.CalCirclePerimeter(3));
            Console.WriteLine(Tool.CalRectangleArea(2, 3));
            Console.WriteLine(Tool.CalRectanglePerimeter(2, 3));
            Console.WriteLine(Tool.CalAbsolute(-2.3f));
        }
    }
}
