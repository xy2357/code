namespace Lesson10_运算符重载
{
    class Position
    {
        public int PosX { get;set; }
        public int PosY { get; set; }

        public static bool operator ==(Position p,Position other)
        {
            if (p.PosX == other.PosX && p.PosY == other.PosY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(Position p, Position other)
        {
            if (p.PosX != other.PosX || p.PosY != other.PosY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    class Vector3
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }

        public static Vector3 operator +(Vector3 p1,Vector3 p2)
        {
            Vector3 p3 = new Vector3();
            p3.PosX = p1.PosX + p2.PosX;
            p3.PosY = p1.PosY + p2.PosY;
            p3.PosZ = p1.PosZ + p2.PosZ;
            return p3;
        }

        public static Vector3 operator -(Vector3 p1, Vector3 p2)
        {
            Vector3 p3 = new Vector3();
            p3.PosX = p1.PosX - p2.PosX;
            p3.PosY = p1.PosY - p2.PosY;
            p3.PosZ = p1.PosZ - p2.PosZ;
            return p3;
        }

        public static Vector3 operator *(Vector3 p1, int num)
        {
            Vector3 p3 = new Vector3();
            p3.PosX = p1.PosX * num;
            p3.PosY = p1.PosY * num;
            p3.PosZ = p1.PosZ * num;
            return p3;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 练习题一
            //Position p1 = new Position();
            //p1.PosX = 1;
            //p1.PosY = 2;
            //Position p2 = new Position();
            //p2.PosX = 1;
            //p2.PosY = 2;

            //Console.WriteLine(p1 == p2);
            //Console.WriteLine(p1 != p2);
            #endregion

            #region 练习题二
            Vector3 p1 = new Vector3();
            p1.PosX = 10;
            p1.PosY = 10;
            p1.PosZ = 10;
            Vector3 p2 = new Vector3();
            p2.PosX = 20;
            p2.PosY = 20;
            p2.PosZ = 20;
            Vector3 p3 = new Vector3();
            p3 = p2 + p1;
            Console.WriteLine(p3.PosX);
            Vector3 p4 = new Vector3();
            p4 = p2 - p1;
            Console.WriteLine(p4.PosX);
            Vector3 p5 = new Vector3();
            p5 = p1 * 3;
            Console.WriteLine(p5.PosX);
            #endregion
        }
    }
}
