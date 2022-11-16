using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.MinecraftPackets;

public static class PacketDataUtils
{
    // For VarInt & VarLong
    private const int VAR_SEGMENT_BITS = 0x7F;
    private const int VAR_CONTINUE_BIT = 0x80;

    [Obsolete("ReadVarInt is deprecated. Use The VarInt struct and VarInt.ReadFrom(stream) method instead!")]
    public static int ReadVarInt(this MemoryStream byteStream)
    {
        return VarInt.ReadFrom(byteStream);
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="byteStream"></param>
    /// <param name="value"></param>
    [Obsolete("WriteVarInt is deprecated. Use The VarInt struct and VarInt.WriteTo(stream) method instead!")]
    public static void WriteVarInt(this MemoryStream byteStream,int value)
    {
        new VarInt(value).WriteTo(byteStream);
    }
    /// <summary>
    /// Evaulates and return the size of the VarInt
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("This method is deprecated, do not use!")]
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

    /// <summary>
    /// Writes a string to the memory stream
    /// </summary>
    /// <param name="bytesStream">The memory stream</param>
    /// <param name="value">The string to write</param>
    public static void WriteVarIntString(this MemoryStream bytesStream, string value)
    {
        new VarInt(value.Length).WriteTo(bytesStream);
        foreach (var b in Encoding.UTF8.GetBytes(value))
        {
            bytesStream.WriteByte(b);
        }
    }
    
    /// <summary>
    /// Read strings prefixed with VarInt as their length
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
        int stringLength = VarInt.ReadFrom(bytes);
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