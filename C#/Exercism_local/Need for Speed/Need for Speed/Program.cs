using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Need_for_Speed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int speed = 10;
            int batteryDrain = 2;
            var car = new RemoteControlCar(speed, batteryDrain);
            int distance = 100;
            var race = new RaceTrack(distance);
            Console.WriteLine(race.TryFinishTrack(car));
        }
    }

    class RemoteControlCar
    {
        // TODO: define the constructor for the 'RemoteControlCar' class
        private int distanceDriven = 0;
        private int battery = 100;
        private int _speed;
        private int _batteryDrain;

        public RemoteControlCar(int speed, int batteryDrain)
        {
            this._speed = speed;
            this._batteryDrain = batteryDrain;
        }

        public bool BatteryDrained()
        {
            if (battery - this._batteryDrain >= 0)
            {
                return false;
            }
            return true;

        }

        public int DistanceDriven()
        {
            return distanceDriven;
        }

        public void Drive()
        {
            if (battery - this._batteryDrain >= 0)
            {
                distanceDriven += this._speed;
                battery -= this._batteryDrain;
            }
        }

        public static RemoteControlCar Nitro()
        {
            return new RemoteControlCar(50, 4);
        }
    }

    class RaceTrack
    {
        // TODO: define the constructor for the 'RaceTrack' class

        private int _distance;
        public RaceTrack(int distance)
        {
            this._distance = distance;
        }

        public bool TryFinishTrack(RemoteControlCar car)
        {
            while (!car.BatteryDrained() && car.DistanceDriven() < _distance)
            {
                car.Drive();
            }
            return car.DistanceDriven() >= _distance;
        }
    }
}
