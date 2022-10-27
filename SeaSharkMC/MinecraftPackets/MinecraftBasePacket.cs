using System;
using Serilog.Core;

namespace SeaSharkMC.MinecraftPackets;

public class MinecraftBasePacket
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

    public MinecraftBasePacket(int packetLength, int packetId, int packetDataOffset, int packetDataLength)
    {
        this.packetLength = packetLength;
        this.packetId = packetId;
        this.packetDataOffset = packetDataOffset;
        this.packetDataLength = packetDataLength;
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
        int idSize = PacketDataUtils.EvaluateVarInt(packetId);
        int packetLength = data.Length + idSize;
        int lengthSize = PacketDataUtils.EvaluateVarInt(packetLength);
        bytesArray = new byte[lengthSize + packetLength];
        PacketDataUtils.WriteVarInt(bytesArray, packetLength);
        PacketDataUtils.WriteVarInt(bytesArray, packetId,lengthSize);
        Array.Copy(data,0,bytesArray,lengthSize+idSize,data.Length);
        return bytesArray;
    }

    /// <summary>
    /// This should return a byte array containing the data part of the packet, aka excluding packet id and length
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual byte[] GetDataByteArray()
    {
        throw new NotImplementedException();
    }

    public int PacketLength => packetLength;

    public int PacketId => packetId;

    public int PacketDataOffset => packetDataOffset;

    public int PacketDataLength => packetDataLength;

    public byte[] BytesArray => bytesArray;
}