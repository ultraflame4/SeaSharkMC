using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog;

namespace SeaSharkMC.Networking.MinecraftPackets;

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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytesBuffer"></param>
    /// <param name="value_"></param>
    /// <param name="offset"></param>
    /// <returns>Number of bytes wrote</returns>
    public static int WriteVarInt(byte[] bytesBuffer,int value, int offset=0)
    {
        int index=0;
        while (true)
        {
            if ((value & ~VAR_SEGMENT_BITS) == 0) {
                bytesBuffer[index + offset] = (byte)(value);
                break;
            }
            
            // Write value -------------Write value using logical & with mask so we dont write the sign bit
            //                                                    |                           /- use mask to set sign bit to true (so it continues)              
            bytesBuffer[index + offset] = (byte)((value & VAR_SEGMENT_BITS) | VAR_CONTINUE_BIT);
            
            index++;

            // bit shift it to the write because the first 7 bits from the left have been written
            value >>=7;
        }

        return index + 1;
    }
    /// <summary>
    /// Evaulates and return the size of the VarInt
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int EvaluateVarInt(int value)
    {
        int index=0;
        while (true)
        {
            if ((value & ~VAR_SEGMENT_BITS) == 0) { break; }
            index++;
            // bit shift it to the write because the first 7 bits from the left have been written
            value >>=7;
        }

        return index + 1;
    }    
    public static int EvaluateVarIntString(string value)
    {
        int sizeA = EvaluateVarInt( value.Length);
        return sizeA + Encoding.UTF8.GetBytes(value).Length;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytesBuffer"></param>
    /// <param name="value"></param>
    /// <returns>Number of bytes written</returns>
    public static int WriteVarIntString(byte[] bytesBuffer, string value, int offset=0)
    {
        int bytesWrote = WriteVarInt(bytesBuffer, value.Length, offset);
        
        int count = 0;
        foreach (var b in Encoding.UTF8.GetBytes(value))
        {
            bytesBuffer[offset + bytesWrote + count] = b;
            count++;
        }
        Log.Debug($"String write VarInt:{bytesWrote}, Count {count+1}");
        return bytesWrote + count;
    }
    
    /// <summary>
    /// Read strings previxed with VarInt as their length
    /// </summary>
    /// <param name="bytes">The byte array to read from</param>
    /// <param name="offset">The byte index to start reading from</param>
    /// <returns>
    /// value - The string that was read
    /// <br/>
    /// totalSize - Total size of the string including the VarInt prefix 
    /// </returns>
    public static (string value, int totalSize) ReadVarIntString(byte[] bytes, int offset)
    {
        // Get string length
        (int stringLength, int stringLengthVarIntSize) = PacketDataUtils.ReadVarInt(bytes, offset); // 1 get address size

        string value = Encoding.UTF8.GetString(bytes, offset+stringLengthVarIntSize, stringLength);
        return (value, stringLengthVarIntSize + stringLength);
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