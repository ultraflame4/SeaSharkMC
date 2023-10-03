using System.IO;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public class LoginSuccessPacket: OutgoingPacket
{
    public readonly string username;
    public readonly byte[] uuid;
    public LoginSuccessPacket(string username, byte[] uuid) : base(0x02)
    {
        this.username = username;
        this.uuid = uuid;
    }

    public override void WriteData(MemoryStream stream)
    {
        stream.Write(uuid);
        new VarIntString(username).WriteTo(stream);
    }
}