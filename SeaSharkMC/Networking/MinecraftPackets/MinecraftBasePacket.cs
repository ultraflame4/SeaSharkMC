using System;
using Serilog.Core;

namespace SeaSharkMC.Networking.MinecraftPackets;

public abstract class MinecraftBasePacket
{

    protected int packetId;
    protected byte[] bytesArray;
    
    public MinecraftBasePacket(MinecraftPacketFrame packetFrame)
    {
        bytesArray = packetFrame.BytesArray;
        packetId = packetFrame.PacketId;


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
    /// This should return a byte array containing the packet's data
    /// </summary>
    protected abstract byte[] GetDataByteArray();

    public int PacketId => packetId;
    
}