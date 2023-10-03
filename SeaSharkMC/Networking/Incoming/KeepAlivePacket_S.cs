using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Incoming;

public class KeepAlivePacket_S
{
    public IncomingPacket packet { get; }
    public readonly long keepAliveId;

    public KeepAlivePacket_S(IncomingPacket packet)
    {
        this.packet = packet;
        keepAliveId = packet.data.ReadLong();
        
    }
}