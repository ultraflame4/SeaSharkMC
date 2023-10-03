using System;
using System.IO;
using System.Net.Sockets;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public abstract class OutgoingPacket
{
    public int packetId;
    protected OutgoingPacket(int packetId) { this.packetId = packetId; }


    public abstract void WriteData(MemoryStream stream);
    public void Write(Stream ns)
    {
        MemoryStream dataStream = new MemoryStream();
        new VarInt(packetId).WriteTo(dataStream);
        WriteData(dataStream);
        int packetLength = (int)dataStream.Length;
        new VarInt(packetLength).WriteTo(ns);
        dataStream.Position = 0;
        dataStream.CopyTo(ns);
    }
}