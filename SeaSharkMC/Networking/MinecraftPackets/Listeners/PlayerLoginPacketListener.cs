using SeaSharkMC.Networking.MinecraftPackets.Client;
using SeaSharkMC.World;

namespace SeaSharkMC.Networking.MinecraftPackets.Listeners;

public class PlayerLoginPacketListener : MinecraftPacketListener
{
    public PlayerLoginPacketListener() : base(0x00) { }

    public override void RecievePacketFrame(MinecraftPacketFrame packetFrame)
    {
        switch (packetFrame.SourceClient.state)
        {
            case ClientState.NONE:
                HandshakePacket handshakePacket = new HandshakePacket(packetFrame);
                logs.Verbose("Server Handshake with {IpAddress}; " +
                             "PacketId: {PacketId}, " +
                             "ProtocolVersion: {handshakePacket.ProtocolVersion} " +
                             "ServerAddress: {handshakePacket.ServerAddress} " +
                             "ServerPort: {handshakePacket.ServerPort} " +
                             "NextState: {handshakePacket.NextState}",
                    packetFrame.SourceClient.IpAddress, packetFrame.PacketId, handshakePacket.ProtocolVersion,
                    handshakePacket.ServerAddress, handshakePacket.ServerPort, handshakePacket.NextState
                );

                packetFrame.SourceClient.state = handshakePacket.NextState;
                break;

            case ClientState.STATUS:
                break;

            case ClientState.LOGIN:
                LoginStartPacket loginStartPacket = new LoginStartPacket(packetFrame);
                logs.Information("Player {Username} attempting to connect from {IP}",
                    loginStartPacket.PlayerUsername, packetFrame.SourceClient.IpAddress);


                // todo maybe add in encryption for online mode
                MinecraftPlayer minecraftPlayer = ServerWorld.getInstance().CreatePlayer(loginStartPacket.PlayerUsername, packetFrame.SourceClient);
                minecraftPlayer.NetworkClient.SendPacket(new LoginSuccessPacket(minecraftPlayer.uuid, minecraftPlayer.Username));
                break;

            default:
                break;
        }
    }
}