using System.IO;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public class SpawnPositionPacket : OutgoingPacket
{
    public readonly Location location;
    public SpawnPositionPacket( Location location) : base(0x42)
    {
        this.location = location;
    }

    public override void WriteData(MemoryStream stream)
    {
        location.WriteTo(stream);
    }
}