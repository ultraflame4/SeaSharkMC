using System.IO;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public class KeepAlivePacket_C: OutgoingPacket
{
    public readonly long keepAliveId;
    public KeepAlivePacket_C(long keepAliveId) : base(0x1F) { this.keepAliveId = keepAliveId; }

    public override void WriteData(MemoryStream stream)
    {
        stream.WriteLong(keepAliveId);
    }
}