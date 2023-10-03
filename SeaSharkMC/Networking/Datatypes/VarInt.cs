using System;
using System.IO;
using System.Net.Sockets;

namespace SeaSharkMC.Networking.Datatypes;

public struct VarInt
{
    private const int SEGMENT_BITS = 0x7F;
    private const int CONTINUE_BIT = 0x80;

    
    private int value = 0;
    public VarInt(int value)
    {
        this.value = value;
    }

    public static implicit operator int(VarInt varInt) => varInt.value;
    public static implicit operator VarInt(int n) => new VarInt(n);

    public void WriteTo(Stream stream)
    {
        while (true)
        {
            if ((value & ~SEGMENT_BITS) == 0)
            {
                stream.WriteByte((byte)(value));
                break;
            }

            // Write value -------------Write value using logical & with mask so we dont write the sign bit
            //                                                    |                           /- use mask to set sign bit to true (so it continues)
            stream.WriteByte((byte)((value & SEGMENT_BITS) | CONTINUE_BIT));

            // bit shift it to the write because the first 7 bits from the left have been written
            value >>= 7;
        }
    }

    /// <summary>
    /// Reads a VarInt from the stream. The stream must be positioned at the start of the VarInt. This method will advance the stream position to the end of the VarInt!
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static VarInt ReadFrom(Stream stream) { return ReadFrom(stream, out _); }

    /// <summary>
    /// Reads a VarInt from the stream. The stream must be positioned at the start of the VarInt. This method will advance the stream position to the end of the VarInt!
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static VarInt ReadFrom(Stream stream, out int size)
    {
        int value = 0;
        size = 0;
        byte currentByte;
        while (true)
        {
            currentByte = (byte)stream.ReadByte();
            value |= currentByte;

            if ((currentByte & CONTINUE_BIT) == 0)
            {
                break;
            }

            size += 1;
            if (size >= 4) throw new ArgumentOutOfRangeException("VarInt is too big");
        }

        return new VarInt(value);
    }
}