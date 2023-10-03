using System;
using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.States;

public class HandshakeStateHandler : StateHandler
{
    public HandshakeStateHandler(PacketManager manager) : base(manager) { }

    public override void HandlePacket(IncomingPacket? packet)
    {
        HandshakePacket handshakePacket = new(packet);
        Log.Verbose(
            "Recieved handshake packet with protocol version {ProtocolVersion}, server address {ServerAddress}, server port {ServerPort} and next state {NextState}",
            handshakePacket.protocolVersion, handshakePacket.serverAddress, handshakePacket.serverPort,
            handshakePacket.nextState);
        manager.SwitchState(handshakePacket.nextState);
    }
}