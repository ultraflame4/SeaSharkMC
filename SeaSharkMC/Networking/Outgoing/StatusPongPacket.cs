using System.IO;
using SeaSharkMC.Networking.Datatypes;
using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.Outgoing;

public class StatusPongPacket : OutgoingPacket
{
    private readonly StatusPingPacket pingPacket;
    public StatusPongPacket(StatusPingPacket pingPacket) : base(0x01) { this.pingPacket = pingPacket; }

    public override void WriteData(MemoryStream stream)
    {
        stream.WriteLong(pingPacket.payload);
    }
}