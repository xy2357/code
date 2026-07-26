using System;

public static class TelemetryBuffer
{
    public static byte[] ToBuffer(long reading)
    {
        byte[] buffer = new byte[9];

        if (reading >= 0 && reading <= ushort.MaxValue)
        {
            buffer[0] = 2;

            byte[] bytes = BitConverter.GetBytes((ushort)reading);

            Array.Copy(bytes, 0, buffer, 1, bytes.Length);
        }
        else if (reading >= short.MinValue && reading < 0)
        {
            buffer[0] = 254;

            byte[] bytes = BitConverter.GetBytes((short)reading);

            Array.Copy(bytes, 0, buffer, 1, bytes.Length);
        }
        else if (reading >= short.MinValue && reading <= int.MaxValue)
        {
            buffer[0] = 252;

            byte[] bytes = BitConverter.GetBytes((int)reading);

            Array.Copy(bytes, 0, buffer, 1, bytes.Length);
        }
        else if (reading >= int.MaxValue + 1L && reading <= uint.MaxValue)
        {
            buffer[0] = 4;

            byte[] bytes = BitConverter.GetBytes((uint)reading);

            Array.Copy(bytes, 0, buffer, 1, bytes.Length);
        }
        else
        {
            buffer[0] = 248;

            byte[] bytes = BitConverter.GetBytes(reading);

            Array.Copy(bytes, 0, buffer, 1, bytes.Length);
        }

        return buffer;
    }


    public static long FromBuffer(byte[] buffer)
    {
        return buffer[0] switch
        {
            2 => BitConverter.ToUInt16(buffer, 1),

            254 => BitConverter.ToInt16(buffer, 1),

            4 => BitConverter.ToUInt32(buffer, 1),

            252 => BitConverter.ToInt32(buffer, 1),

            248 => BitConverter.ToInt64(buffer, 1),

            _ => 0
        };
    }
}