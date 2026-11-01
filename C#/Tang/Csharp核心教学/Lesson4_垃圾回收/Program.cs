namespace Lesson4_垃圾回收
{
    using System.Runtime.CompilerServices;

    internal class Program
    {
        // 演示用：读出引用变量里存的地址（正式代码里不要这么用）
        static string Addr(object o) => Unsafe.As<object, IntPtr>(ref o).ToString("X");

        static void Show(string label, Person p)
        {
            Console.WriteLine($"{label}：第{GC.GetGeneration(p)}代，地址 {Addr(p)}，name = {p.name}");
        }

        static void Main(string[] args)
        {
            Person p = new Person();
            Show("刚创建 ", p);
            GC.Collect();
            Show("GC 一次", p);
            GC.Collect();
            Show("GC 两次", p);
        }
    }

    class Person
    {
        public string name = "小李";
    }
}
