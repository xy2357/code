using System.Timers;

namespace EventSample10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            Boy boy = new Boy();
            Girl girl = new Girl();
            timer.Elapsed += boy.Action;
            timer.Elapsed += girl.Action;
            timer.Start();
            Console.ReadLine();
        }
    }

    class Boy
    {
        internal void Action(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Jump!");
        }
    }

    class Girl
    {
        internal void Action(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Love!");
        }
    }
}
