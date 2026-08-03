using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Control_Car
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RemoteControlCar remoteControlCar = new RemoteControlCar();
            remoteControlCar.Drive();
            remoteControlCar.Drive();
            remoteControlCar.Drive();
            string distanceDisplay = remoteControlCar.DistanceDisplay();
            string batteryDisplay = remoteControlCar.BatteryDisplay();
            Console.WriteLine(distanceDisplay);
            Console.WriteLine(batteryDisplay);
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
