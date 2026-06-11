using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventSample15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Waiter waiter = new Waiter();
            customer.Order += waiter.Action;
            customer.Action();

            //OrderEventArgs e1 = new OrderEventArgs();
            //e1.DishName = "Manhanquanxi";
            //e1.Size = "small";
            //OrderEventArgs e2 = new OrderEventArgs();
            //e2.DishName = "Beer";
            //e2.Size = "large";
            //Customer badGuy = new Customer();
            //badGuy.Order += waiter.Action;
            //badGuy.Order.Invoke(customer, e1);
            //badGuy.Order.Invoke(customer, e2);

            customer.PayTheBill();
        }
    }

    public class OrderEventArgs:EventArgs
    {
        public string DishName {  get; set; }
        public string Size { get; set; }
    }

    //public delegate void OrderEventHandle(Customer customer, OrderEventArgs e);

    public class Customer
    {
        //private OrderEventHandle orderEventHandle;

        //public event OrderEventHandle Order
        //{
        //    add 
        //    {
        //        this.orderEventHandle += value;
        //    }
        //    remove
        //    {
        //        this.orderEventHandle -= value;
        //    }
        //}

        public event EventHandler Order;


        //public OrderEventHandle Order;

        public double Bill {  get; set; }
        public void PayTheBill()
        {
            Console.WriteLine("I will pay ${0}", this.Bill);
        }

        public void WalkIn()
        {
            Console.WriteLine("Walk into the restaurant");
        }

        public void SitDown()
        {
            Console.WriteLine("Sit Down");
        }

        public void Think()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Let me think ...");
                Thread.Sleep(1000);
            }

            if (this.Order != null)
            {
                OrderEventArgs e = new OrderEventArgs();
                e.DishName = "Burger";
                e.Size = "large";
                this.Order.Invoke(this, e);
            }
        }

        public void Action()
        {
            Console.ReadLine();
            this.WalkIn();
            this.SitDown();
            this.Think();
        }
    }

    class Waiter
    {
        public void Action(object sender, EventArgs e)
        {
            Customer customer = sender as Customer;
            OrderEventArgs orderInfo = e as OrderEventArgs;

            Console.WriteLine("I will serve you the dish - {0}", orderInfo.DishName);
            double price = 10;
            switch (orderInfo.Size)
            {
                case "small":
                    price = price * 0.5;
                    break;
                case "large":
                    price = price * 1.5;
                    break;
                default:
                    break;
            }

            customer.Bill += price;
        }
    }
}
