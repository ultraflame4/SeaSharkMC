using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Incoming;

public class LoginStartPacket
{
    public IncomingPacket packet { get; }
    public readonly string playerUsername;

    public LoginStartPacket(IncomingPacket packet)
    {
        this.packet = packet;
        playerUsername = VarIntString.ReadFrom(packet.data);
    }
}