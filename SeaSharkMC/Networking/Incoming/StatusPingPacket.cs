using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Incoming;

public class StatusPingPacket
{
    public readonly long payload;
    public IncomingPacket packet { get; }

    public StatusPingPacket(IncomingPacket packet)
    {
        this.packet = packet; 
        payload = packet.data.ReadLong();
    }
}