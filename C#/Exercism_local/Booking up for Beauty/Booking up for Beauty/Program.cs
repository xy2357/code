using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_up_for_Beauty
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Appointment.Schedule("7/25/2019 13:45:00"));
        }
    }

    static class Appointment
    {
        public static DateTime Schedule(string appointmentDateDescription)
        {
            return DateTime.Parse(appointmentDateDescription);
        }

        public static bool HasPassed(DateTime appointmentDate)
        {
            return appointmentDate < DateTime.Now;
        }

        public static bool IsAfternoonAppointment(DateTime appointmentDate)
        {
            return appointmentDate.Hour >= 12 && appointmentDate.Hour < 18;
        }

        public static string Description(DateTime appointmentDate)
        {
            return $"You have an appointment on {appointmentDate}.";
        }

        public static DateTime AnniversaryDate()
        {
            return new DateTime(DateTime.Now.Year, 9, 15);
        }
    }
}
