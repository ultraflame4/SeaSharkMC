using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.States;

public class HandshakeStateHandler : IStateHandler
{
    public void HandlePacket(IncomingPacket packet)
    {
        HandshakePacket handshakePacket = new(packet);
        
    }
}