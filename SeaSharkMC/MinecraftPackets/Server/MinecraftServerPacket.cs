using System;

namespace SeaSharkMC.MinecraftPackets;

public class MinecraftServerPacket : MinecraftBasePacket
{
    public MinecraftServerPacket(byte[] bytesArray) : base(bytesArray) { }

    protected override byte[] GetDataByteArray()
    {
        byte[] data = new byte[packetDataLength];
        Array.Copy(bytesArray, packetDataOffset, data, 0, packetDataLength);
        return data;
    }
}