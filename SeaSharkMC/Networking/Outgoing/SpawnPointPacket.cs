using System.IO;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public class SpawnPointPacket : OutgoingPacket
{
    public readonly Location location;
    public SpawnPointPacket( Location location) : base(0x42)
    {
        this.location = location;
    }

    public override void WriteData(MemoryStream stream)
    {
        location.WriteTo(stream);
    }
}