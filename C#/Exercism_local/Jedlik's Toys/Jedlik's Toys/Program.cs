using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Jedlik_s_Toys
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var car = new RemoteControlCar();
            for (var i = 0; i < 100; i++)
            {
                car.Drive();
            }
            car.Drive();
            Console.WriteLine(car.DistanceDisplay());
            //car.DistanceDisplay();
        }
    }

    class RemoteControlCar
    {
        private int count = 0;
        private int battery = 100;
        public static RemoteControlCar Buy()
        {
            return new RemoteControlCar();
            
        }

        public string DistanceDisplay()
        {
            if (battery > 0)
            {
                int distance = 20 * (100 - battery);
                return "Driven " + distance + " meters";
            }
            else
            {
                return "Driven " + 100 * 20 + " meters";
            }
        }

        public string BatteryDisplay()
        {
            if (battery > 0)
            {
                return "Battery at " + battery + "%";
            }
            else
            {
                return "Battery empty";
            }
        }

        public void Drive()
        {
            if (battery > 0)
            {
                battery--;
            }
        }
    }
}
