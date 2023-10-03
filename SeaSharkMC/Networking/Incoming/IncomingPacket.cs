using System;
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

    public static IncomingPacket? Read(NetworkStream stream)
    {

        int length = VarInt.ReadFrom(stream);
        if (length == 0) return null;
        int packetId = VarInt.ReadFrom(stream, out int idLength);
        Console.WriteLine(packetId + " " + length);
        MemoryStream data = new MemoryStream(length - idLength);
        byte[] buffer = new byte[data.Capacity];
        stream.Read(buffer,0, buffer.Length);
        data.Write(buffer,0,buffer.Length);
        data.Position = 0;
        Console.WriteLine(BitConverter.ToString(buffer).Replace("-",string.Empty));
        IncomingPacket? packet = new IncomingPacket(length,packetId,data); 
        
        return packet;
    }
}