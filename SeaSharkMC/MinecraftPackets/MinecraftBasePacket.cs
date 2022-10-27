
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

    public byte ReadDataByte(int index)
    {
        return bytesArray[index];
    }

    public int PacketLength => packetLength;

    public int PacketId => packetId;

    public int PacketDataOffset => packetDataOffset;

    public int PacketDataLength => packetDataLength;

    public byte[] BytesArray => bytesArray;
}