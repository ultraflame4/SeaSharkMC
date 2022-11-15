using System;
using System.IO;
using System.Linq;
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

    public static int ReadVarInt(this MemoryStream byteStream)
    {
        int value = 0;
        int position = 0;
        byte currentByte;
        while (true)
        {
            currentByte = (byte)byteStream.ReadByte();
            value |= currentByte;
            
            if ((currentByte & VAR_CONTINUE_BIT) == 0)
            {
                break;
            }
            position += 7;
            if (position >= 32) throw new ArgumentOutOfRangeException("VarInt is too big");
        }

        return value;
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="byteStream"></param>
    /// <param name="value"></param>
    public static void WriteVarInt(this MemoryStream byteStream,int value)
    {
        while (true)
        {
            if ((value & ~VAR_SEGMENT_BITS) == 0) {
                
                byteStream.WriteByte((byte)(value));
                break;
            }
            // Write value -------------Write value using logical & with mask so we dont write the sign bit
            //                                                    |                           /- use mask to set sign bit to true (so it continues)
            byteStream.WriteByte((byte)((value & VAR_SEGMENT_BITS) | VAR_CONTINUE_BIT));

            // bit shift it to the write because the first 7 bits from the left have been written
            value >>=7;
        }
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
    public static void WriteVarIntString(this MemoryStream bytesStream, string value)
    {
        WriteVarInt(bytesStream, value.Length);
        foreach (var b in Encoding.UTF8.GetBytes(value))
        {
            bytesStream.WriteByte(b);
        }
    }
    
    /// <summary>
    /// Read strings previxed with VarInt as their length
    /// </summary>
    /// <param name="bytes">The byte stream to read from</param>
    /// <returns>
    /// value - The string that was read
    /// <br/>
    /// totalSize - Total size of the string including the VarInt prefix 
    /// </returns>
    public static string ReadVarIntString(this MemoryStream bytes)
    {
        // Get string length
        int stringLength = ReadVarInt(bytes);
        byte[] buffer = new byte[stringLength];
        bytes.Read(buffer, 0, stringLength);
        string value = Encoding.UTF8.GetString(buffer,0,stringLength);
        return value;
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