using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.States;

public interface IStateHandler
{
    public void HandlePacket(IncomingPacket packet);
}