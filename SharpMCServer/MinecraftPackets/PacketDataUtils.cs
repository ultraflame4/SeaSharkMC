using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using Serilog;


public static class PacketDataUtils
{
    // For VarInt & VarLong
    private const int VAR_SEGMENT_BITS = 0x7F;
    private const int VAR_CONTINUE_BIT = 0x80;

    public static (int value, int size) ReadVarInt(byte[] byteArray, int offset = 0)
    {
        int value = 0;
        int position = 0;
        int index = 0;
        byte currentByte;

        while (true)
        {
            currentByte = byteArray[index + offset];
            value |= currentByte;

            if ((currentByte & VAR_CONTINUE_BIT) == 0)
            {
                break;
            }

            position += 7;
            index++;
            if (position >= 32) throw new ArgumentOutOfRangeException("VarInt is too big");
        }
        return (value, index + 1);
    }

    public static string ConvertBytesToHex(this byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", String.Empty);
    }

    public static string GetIpAddress(this TcpClient client)
    {
        return (client.Client.LocalEndPoint as IPEndPoint).Address.ToString();
    }
}