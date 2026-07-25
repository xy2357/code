using System.Formats.Asn1;

namespace Hyper_optimized_Telemetry_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public static class TelemetryBuffer
    {
        public static byte[] ToBuffer(long reading)
        {
            byte[] buffer = new byte[9];

            //ushort
            if (reading >=0 && reading <=ushort.MaxValue)
            {
                buffer[0] = 5;
                byte[] payload = BitConverter.GetBytes(reading);
                Array.Copy(payload, 0, buffer, 1, payload.Length);
            }
            //short
            else if (reading >=short.MinValue && reading < 0)
            {
                buffer[0] = 0xfe;
                byte[] payload = BitConverter.GetBytes((short)reading);
                Array.Copy(payload, 0, buffer, 1, payload.Length);
            }
            //int
            else if (reading <= int.MaxValue && reading >= int.MinValue)
            {
                buffer[0] = 0xfc;
                byte[] payload = BitConverter.GetBytes((short)reading);
                Array.Copy(payload, 0, buffer, 1, payload.Length);
            }
            //unit
            else if (reading < UInt32.MaxValue)
            {
                buffer[0] = 4;
                byte[] payload = BitConverter.GetBytes((short)reading);
                Array.Copy(payload, 0, buffer, 1, payload.Length);
            }
            else if (reading >= short.MinValue && reading < 0)
            {
                buffer[0] = 0xfe;
                byte[] payload = BitConverter.GetBytes((short)reading);
                Array.Copy(payload, 0, buffer, 1, payload.Length);
            }

            return buffer;
        }

        public static long FromBuffer(byte[] buffer)
        {
            throw new NotImplementedException("Please implement the static TelemetryBuffer.FromBuffer() method");
        }
    }

}
