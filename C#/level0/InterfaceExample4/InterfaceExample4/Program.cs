namespace InterfaceExample4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Driver driver = new Driver(new Car());
            driver.Drive();
        }
    }

    class Driver
    {
        private IVehicle _vehicle;
        public Driver(IVehicle vehicle)
        {
            _vehicle = vehicle;
        }

        public void Drive()
        {
            _vehicle.Run();
        }
    }

    interface IVehicle
    {
        void Run();
    }

    class Car : IVehicle
    {
        public void Run()
        {
            Console.WriteLine("Car is runnning...");
        }
    }

    class Trunk : IVehicle
    {
        public void Run()
        {
            Console.WriteLine("Trunk is runnning...");
        }
    }


    interface IWeapon
    {
        void Fire();
    }
    interface ITank:IVehicle,IWeapon
    {
    }

    class LightTank : ITank
    {
        public void Fire()
        {
            Console.WriteLine("Boom!");
        }

        public void Run()
        {
            Console.WriteLine("Ka Ka Ka ...");
        }
    }

    class MediumTank : ITank
    {
        public void Fire()
        {
            Console.WriteLine("Boom!!");
        }

        public void Run()
        {
            Console.WriteLine("Ka! Ka! Ka! ...");
        }
    }

    class HeavyTank : ITank
    {
        public void Fire()
        {
            Console.WriteLine("Boom!!!");
        }

        public void Run()
        {
            Console.WriteLine("Ka!! Ka!! Ka!! ...");
        }
    }

}
