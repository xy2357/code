using System.ComponentModel;

namespace Hyper_optimized_Telemetry;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(TelemetryBuffer.ToBuffer(5));
    }
    
    public static class TelemetryBuffer
    {
        public static byte[] ToBuffer(long reading)
        {
            byte[] buffer = new byte[9];
            if (reading >= 0 && reading <= ushort.MaxValue)
            {
                buffer[0] = 2;
                byte[] payload = BitConverter.GetBytes((ushort)reading);
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