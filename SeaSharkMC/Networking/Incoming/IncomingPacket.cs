using System;
using System.IO;
using System.Net.Sockets;
using SeaSharkMC.Networking.Datatypes;

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
        // if (length == 0) return null;
        int packetId = VarInt.ReadFrom(stream, out int idSize);
        MemoryStream data = new MemoryStream(length - idSize);
        byte[] buffer = new byte[data.Capacity];
        stream.Read(buffer,0, buffer.Length);
        data.Write(buffer,0,buffer.Length);
        data.Position = 0;
        // Console.WriteLine(BitConverter.ToString(buffer).Replace("-",string.Empty));
        IncomingPacket? packet = new IncomingPacket(length,packetId,data); 
        
        return packet;
    }
}