using System;
using Serilog.Core;

namespace SeaSharkMC.Networking.MinecraftPackets;

public abstract class MinecraftBasePacket
{
    protected int packetLength;
    protected int packetId;
    protected int packetDataOffset;
    protected int packetDataLength;
    protected byte[] bytesArray;

    public MinecraftBasePacket(byte[] bytesArray)
    {
        this.bytesArray = bytesArray;
        (packetLength, int aSize) = PacketDataUtils.ReadVarInt(bytesArray);
        (packetId, int bSize) = PacketDataUtils.ReadVarInt(bytesArray, aSize);

        packetDataOffset = aSize + bSize;
        packetDataLength = packetLength - bSize;
    }

    public MinecraftBasePacket(int packetId)
    {
        this.packetId = packetId;
    }

    public byte ReadDataByte(int index)
    {
        return bytesArray[index];
    }

    /// <summary>
    /// Formats the bytearray and returns it. Note that the returned object IS NOT a copy
    /// This may change internal values
    /// </summary>
    /// <returns></returns>
    public byte[] ToBytesArray()
    {
        byte[] data = GetDataByteArray();
        packetDataLength = data.Length;
        int idSize = PacketDataUtils.EvaluateVarInt(packetId);
        packetLength = data.Length + idSize;
        int lengthSize = PacketDataUtils.EvaluateVarInt(packetLength);
        packetDataOffset = idSize + lengthSize;
        
        bytesArray = new byte[lengthSize + packetLength];
        PacketDataUtils.WriteVarInt(bytesArray, packetLength);
        PacketDataUtils.WriteVarInt(bytesArray, packetId,lengthSize);
        Array.Copy(data,0,bytesArray,lengthSize+idSize,data.Length);
        return bytesArray;
    }

    /// <summary>
    /// This should return a byte array containing the packet's data
    /// </summary>
    protected abstract byte[] GetDataByteArray();
    

    public int PacketLength => packetLength;

    public int PacketId => packetId;

    public int PacketDataOffset => packetDataOffset;

    public int PacketDataLength => packetDataLength;

    public byte[] BytesArray => bytesArray;
}