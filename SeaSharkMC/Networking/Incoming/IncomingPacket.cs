using System.Net.Sockets;
using SeaSharkMC.old.Networking.Datatypes;
using SeaSharkMC.old.Networking.MinecraftPackets;

namespace SeaSharkMC.Networking.Incoming;

/// <summary>
/// Represents a packet that is incoming from a client
/// </summary>
public class IncomingPacket
{
    public int length { get; init; }
    public int packetId { get; init; }
    public byte[] data { get; init; }
    public static IncomingPacket Read(NetworkStream stream)
    {
        int length = VarInt.ReadFrom(stream);
        int packetId = VarInt.ReadFrom(stream);
        int idLength = PacketDataUtils.EvaluateVarInt(packetId);
        byte[] data = new byte[length - idLength];
        stream.ReadExactly(data, 0, data.Length);
        
        IncomingPacket packet = new IncomingPacket() {
                length = length,
                packetId = packetId,
                data = data
        };
        
        return packet;
    }
}