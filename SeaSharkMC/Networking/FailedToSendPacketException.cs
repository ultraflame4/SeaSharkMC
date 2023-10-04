using System;
using System.IO;
using SeaSharkMC.Networking.Datatypes;
using SeaSharkMC.Networking.Outgoing;

namespace SeaSharkMC.Networking;

public class FailedToSendPacketException : IOException
{
    public IOException? innerException { get; }
    public string hexData { get; }
    public OutgoingPacket packet { get; }
    public Type packetType => packet.GetType();
    public int packetId => packet.packetId;

    public FailedToSendPacketException(OutgoingPacket packet, IOException? innerException=null)
            : base($"Failed to send packet of type {packet.GetType().Name} and id {packet.packetId}!")
    {
        this.packet = packet;
        this.innerException = innerException;
        MemoryStream stream = new MemoryStream();
        packet.WriteData(stream);
        hexData = stream.HexDump();
    }
}