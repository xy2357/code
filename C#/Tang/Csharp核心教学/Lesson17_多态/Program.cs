namespace Lesson17_多态
{
    class Duck
    {
        public virtual void Call()
        {

        }
    }

    class TrueDuck : Duck
    {
        public override void Call()
        {
            Console.WriteLine("嘎嘎嘎");
        }
    }

    class WoodenDuck : Duck
    {
        public override void Call()
        {
            Console.WriteLine("吱吱吱");
        }
    }

    class RubberDuck : Duck
    {
        public override void Call()
        {
            Console.WriteLine("唧唧唧");
        }
    }

    class Worker
    {
        public virtual void ClockInTime()
        {
            Console.WriteLine("打卡时间是9点");
        }
    }

    class Manager : Worker
    {
        public override void ClockInTime()
        {
            Console.WriteLine("打卡时间是11点");
        }
    }

    class Programmer : Worker
    {
        public override void ClockInTime()
        {
            Console.WriteLine("不用打卡");
        }
    }

    class Shapes
    {
        public virtual float CalArea()
        {
            return 0;
        }

        public virtual float CalPerimeter()
        {
            return 0;
        }
    }

    class Rectangle : Shapes
    {
        public float w;
        public float h;

        public Rectangle(float w, float h)
        {
            this.w = w;
            this.h = h;
        }
        public override float CalArea()
        {
            return w * h;
        }
        public override float CalPerimeter()
        {
            return (w + h) * 2f;
        }
    }
    class Square : Shapes
    {
        public float h;
        public Square(float h)
        {
            this.h = h;
        }
        public override float CalArea()
        {
            return h * h;
        }
        public override float CalPerimeter()
        {
            return h * 4f;
        }
    }

    class Circle : Shapes
    {
        public float r;
        public Circle(float r)
        {
            this.r = r;
        }
        public override float CalArea()
        {
            return r * r * 3.14f;
        }
        public override float CalPerimeter()
        {
            return r * 2f * 3.14f;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Duck d1 = new TrueDuck();
            d1.Call();
            Duck d2 = new WoodenDuck();
            d2.Call();
            Duck d3 = new RubberDuck();
            d3.Call();

            Worker w1 = new Worker();
            w1.ClockInTime();
            Worker w2 = new Manager();
            w2.ClockInTime();
            Worker w3 = new Programmer();
            w3.ClockInTime();

            Shapes s1 = new Rectangle(2, 3);
            Console.WriteLine(s1.CalArea());
            Console.WriteLine(s1.CalPerimeter());
            Shapes s2 = new Square(5);
            Console.WriteLine(s2.CalArea());
            Console.WriteLine(s2.CalPerimeter());
            Shapes s3 = new Circle(3);
            Console.WriteLine(s3.CalArea());
            Console.WriteLine(s3.CalPerimeter());
        }
    }
}
