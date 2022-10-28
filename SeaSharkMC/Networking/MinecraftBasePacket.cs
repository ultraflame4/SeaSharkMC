using System;
using System.Collections.Generic;
using System.IO;
using SeaSharkMC.MinecraftPackets;

namespace SeaSharkMC.Networking;

public class MinecraftBasePacket
{
    protected int packetId;
    protected byte[] dataBytes;

    public int PacketId => packetId;
    public byte[] DataBytes => dataBytes;

    private MinecraftBasePacket(int packetId, byte[] dataBytes)
    {
        this.packetId = packetId;
        this.dataBytes = dataBytes;
    }

    /// <summary>
    /// Creates a packet from the data provided
    /// </summary>
    /// <param name="packetId"></param>
    /// <param name="dataBytes"></param>
    /// <returns></returns>
    public static MinecraftBasePacket Create(int packetId, byte[] dataBytes)
    {
        return new MinecraftBasePacket(packetId, dataBytes);
    }

    /// <summary>
    /// Creates a single packet from an array of bytes
    /// </summary>
    /// <param name="bytesArray">The byte array to read from</param>
    /// <param name="bytesRead">Variable to write the number of bytes read to.</param>
    /// <param name="offset">The position of the byte array to start reading from</param>
    /// <returns></returns>
    public static MinecraftBasePacket FromBytes(byte[] bytesArray, out int bytesRead, int offset = 0)
    {
        (int packetLength, int aSize) = PacketDataUtils.ReadVarInt(bytesArray, offset);
        (int packetId, int bSize) = PacketDataUtils.ReadVarInt(bytesArray, aSize + offset);
        int dataLength = packetLength - bSize;
        byte[] dataBuffer = new byte[dataLength];
        Array.Copy(bytesArray, aSize + bSize + offset, dataBuffer, 0, dataLength);
        bytesRead = aSize + packetLength;

        return MinecraftBasePacket.Create(packetId, dataBuffer);
    }

    /// <summary>
    /// Creates a many packet from an array of bytes
    /// </summary>
    /// <param name="bytesArray">The byte array to read from</param>
    /// <param name="bytesRead">Variable to write the number of bytes read to.</param>
    /// <param name="offset">The position of the byte array to start reading from</param>
    /// <returns></returns>
    public static MinecraftBasePacket[] ManyFromBytes(byte[] bytesArray, out int bytesRead, int offset = 0)
    {
        var packets = new List<MinecraftBasePacket>();
        int packetOffset = offset;

        while (true)
        {
            int packetSize;
            var packet = MinecraftBasePacket.FromBytes(bytesArray, out packetSize, packetOffset);
            if (packetSize < 2) break;
            
            packets.Add(packet);
            packetOffset += packetSize;
            
            if (offset>=bytesArray.Length-1) break;
            
        }

        bytesRead = packetOffset-offset;
        
        return packets.ToArray();
    }
    
    /// <summary>
    /// Creates a single packet from an array of bytes
    /// </summary>
    /// <param name="bytesArray">The byte array to read from</param>
    /// <param name="offset">The position of the byte array to start reading from</param>
    /// <returns></returns>
    public static MinecraftBasePacket FromBytes(byte[] bytesArray, int offset = 0)
    {
        return FromBytes(bytesArray, out int _, offset);
    }
    /// <summary>
    /// Creates a many packet from an array of bytes
    /// </summary>
    /// <param name="bytesArray">The byte array to read from</param>
    /// <param name="offset">The position of the byte array to start reading from</param>
    /// <returns></returns>
    public static MinecraftBasePacket[] ManyFromBytes(byte[] bytesArray, int offset = 0)
    {
        return ManyFromBytes(bytesArray, out int _, offset);
    }
}