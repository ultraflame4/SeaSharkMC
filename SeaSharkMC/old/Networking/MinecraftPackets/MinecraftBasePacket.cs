using System.IO;

namespace SeaSharkMC.old.Networking.MinecraftPackets;

public abstract class MinecraftBasePacket
{

    protected int packetId;
    public int PacketId => packetId;

    public MinecraftBasePacket(GenericMinecraftPacket packet)
    {
        packetId = packet.PacketId;
    }

    public MinecraftBasePacket(int packetId)
    {
        this.packetId = packetId;
    }


    /// <summary>
    /// Formats the bytearray and returns it. Note that the returned object IS NOT a copy
    /// This may change internal values
    /// </summary>
    /// <returns></returns>
    public byte[] ToBytesArray()
    {
        MemoryStream dataStream = new MemoryStream();
        PacketDataUtils.WriteVarInt(dataStream,packetId);
        OnDataToBytes(dataStream);
        int packetLength = (int)dataStream.Length;

        MemoryStream packetBytes = new MemoryStream();
        PacketDataUtils.WriteVarInt(packetBytes,packetLength);
        dataStream.CopyTo(packetBytes);
        return packetBytes.ToArray();
    }

    /// <summary>
    /// This should return a byte array containing the packet's data
    /// </summary>
    protected abstract void OnDataToBytes(MemoryStream dataStream);


}