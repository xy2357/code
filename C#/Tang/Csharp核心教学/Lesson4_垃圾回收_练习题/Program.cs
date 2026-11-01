namespace Lesson4_垃圾回收_练习题
{
    class Ticket
    {
        public uint distance;
        public float price;

        public Ticket(uint distance)
        {
            this.distance = distance;
            price = GetPrice();
        }

        private float GetPrice()
        {
            if (distance <= 100)
            {
                return (float)(1 * distance);
            }
            else if (distance <= 200 )
            {
                return (float)(1 * distance * 0.95f);
            }
            else if (distance <= 300)
            {
                return (float)(1 * distance * 0.9f);
            }
            else
            {
                return (float)(1 * distance * 0.8f);
            }
        }

        public void ShowTicket()
        {
            Console.WriteLine("{0}公里{1}块钱",distance,price);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Ticket t1 = new Ticket(400);
            t1.ShowTicket();
        }
    }
}
