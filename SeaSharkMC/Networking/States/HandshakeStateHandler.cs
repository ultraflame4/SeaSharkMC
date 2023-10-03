using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.States;

public class HandshakeStateHandler : StateHandler
{
    public HandshakeStateHandler(PacketManager manager) : base(manager) { }

    public override void HandlePacket(IncomingPacket packet)
    {
        HandshakePacket handshakePacket = new(packet);
        manager.SwitchState(handshakePacket.nextState);
    }
}