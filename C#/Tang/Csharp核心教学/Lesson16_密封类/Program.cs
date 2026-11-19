namespace Lesson16_密封类
{
    class Vehicle
    {
        private Person?[] drivers;
        private Person?[] passengers;

        public Vehicle(int speed, int maxSpeed, int maxCapacity)
        {
            Speed = speed;
            MaxSpeed = maxSpeed;
            MaxCapacity = maxCapacity;
            drivers = new Person?[1];
            passengers = new Person?[maxCapacity - 1];
        }

        public int Speed { get; set; }
        public int MaxSpeed { get; }
        public int MaxCapacity { get; }

        public Person?[] Drivers
        {
            get { return drivers; }
        }

        public Person?[] Passengers
        {
            get { return passengers; }
        }

        public void GetOn(Person p)
        {
            if (p is Driver)
            {
                if (drivers[0] == null)
                {
                    drivers[0] = p;
                }
                else
                {
                    Console.WriteLine("已经有司机了");
                }
            }
            else if (p is Passenger)
            {
                for (int i = 0; i < passengers.Length; i++)
                {
                    if (ReferenceEquals(passengers[i], p))
                    {
                        Console.WriteLine("这位乘客已在车上");
                        return;
                    }
                }

                for (int i = 0; i < passengers.Length; i++)
                {
                    if (passengers[i] == null)
                    {
                        passengers[i] = p;
                        return;
                    }
                }
                Console.WriteLine("乘客作为已满");
            }
        }

        public void GetOff(Person p)
        {
            if (p is Driver)
            {
                if (!ReferenceEquals(drivers[0], p))
                {
                    Console.WriteLine("司机不在车上");
                    return;
                }
                if (!ArePassengerSeatsEmpty())
                {
                    Console.WriteLine("车上还有乘客，司机不可下车！");
                    return;
                }

                drivers[0] = null;
                Console.WriteLine("{0}已下车",p.Name);
            }
            else if (p is Passenger)
            {
                for (int i = 0; i < this.passengers.Length; i++)
                {
                    if (ReferenceEquals(passengers[i], p))
                    {
                        passengers[i] = null;
                        Console.WriteLine("{0}已下车",p.Name);
                        return;
                    }
                }
                Console.WriteLine("这位乘客不在车上");
            }
        }

        public void Drive()
        {
            if (drivers[0] != null && drivers[0] is Driver)
            {
                Console.WriteLine("滴滴滴");
            }
            else
            {
                Console.WriteLine("车上没有司机");
            }
        }

        public void CarAccident()
        {
            int count = 0;

            if (drivers[0] != null)
            {
                count++;
            }

            for (int i = 0; i < passengers.Length; i++)
            {
                if (passengers[i] != null)
                {
                    count += 1;
                }
            }
            Console.WriteLine("车上{0}人死亡", count);
        }

        public bool ArePassengerSeatsEmpty()
        {
            for (int i = 0; i < this.passengers.Length; i++)
            {
                if (passengers[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ArePassengerSeatsFull()
        {
            for (int i = 0; i < this.passengers.Length; i++)
            {
                if (passengers[i] == null)
                {
                    return false;
                }
            }
            return true;
        }
    }

    class Car : Vehicle
    {
        public  Car() : base(80,120,5)
        {

        }
    }
    class Bus : Vehicle
    {
        public Bus() : base(60, 100, 20)
        {

        }
    }

    class Person
    {
        public string Name { get; set; }
        public Person(string name)
        {
            Name = name;
        }
    }

    class Driver : Person
    {
        public Driver(string name) : base(name)
        {

        }
    }

    class Passenger : Person
    {
        public Passenger(string name) : base(name)
        {

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Car c = new Car();
            Driver d = new Driver("王司机");
            c.GetOn(d);
            Console.WriteLine(c.Drivers[0].Name);

            Passenger p1 = new Passenger("小李");
            c.GetOn(p1);
            Console.WriteLine(c.Passengers[0].Name);
            c.GetOff(p1);

            Passenger p2 = new Passenger("小周");
            c.GetOn(p2);
            c.CarAccident();
        }
    }
}
