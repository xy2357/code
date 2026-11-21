namespace 抽象类
{
    abstract class Animal
    {
        public abstract void Speak();
    }

    class Person : Animal
    {
        public override void Speak()
        {
            Console.WriteLine("我是人");
        }
    }

    class Dog : Animal
    {
        public override void Speak()
        {
            Console.WriteLine("汪汪汪");
        }
    }

    class Cat : Animal
    {
        public override void Speak()
        {
            Console.WriteLine("喵喵喵");
        }
    }

    abstract class Graphic
    {
        public abstract float GetArea();
        public abstract float GetPermiter();
    }

    class Rectangle : Graphic
    {
        public float w;
        public float h;

        public Rectangle(int w, int h)
        {
            this.w = w;
            this.h = h;
        }
        public override float GetArea()
        {
            return w * h;
        }

        public override float GetPermiter()
        {
            return (w + h) * 2f;
        }
    }

    class Square : Graphic
    {
        public float w;

        public Square(int w)
        {
            this.w = w;
        }
        public override float GetArea()
        {
            return w * w;
        }

        public override float GetPermiter()
        {
            return (w + w) * 2f;
        }
    }

    class Circle : Graphic
    {
        public float r;

        public Circle(int r)
        {
            this.r = r;
        }
        public override float GetArea()
        {
            return r * r * 3.14f;
        }

        public override float GetPermiter()
        {
            return r * 2 * 3.14f;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person();
            p.Speak();
            Dog d = new Dog();
            d.Speak();
            Cat c = new Cat();
            c.Speak();
        }
    }
}
