using System.IO;
using System.Net.Sockets;
using SeaSharkMC.Networking.Datatypes;
using SeaSharkMC.old.Networking.Datatypes;
using SeaSharkMC.old.Networking.MinecraftPackets;

namespace SeaSharkMC.Networking.Incoming;

/// <summary>
/// Represents a packet that is incoming from a client
/// </summary>
public class IncomingPacket
{
    public int length { get; }
    public int packetId { get;}
    public MemoryStream data { get;  }
    public IncomingPacket(int length, int packetId, MemoryStream data)
    {
        this.length = length;
        this.packetId = packetId;
        this.data = data;
    }

    public static IncomingPacket Read(NetworkStream stream)
    {
        int length = VarInt.ReadFrom(stream);
        int packetId = VarInt.ReadFrom(stream, out int idLength);
        MemoryStream data = new MemoryStream(length - idLength);
        stream.CopyTo(data, data.Capacity);
        data.Position = 0;
        IncomingPacket packet = new IncomingPacket(length,packetId,data); 
        
        return packet;
    }
}