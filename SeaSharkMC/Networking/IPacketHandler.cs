using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking;

public interface IPacketHandler
{
    public void HandlePacket(IncomingPacket packet);
}