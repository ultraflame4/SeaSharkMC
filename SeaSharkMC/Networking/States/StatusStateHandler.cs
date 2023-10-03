using SeaSharkMC.Game;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.Outgoing;

namespace SeaSharkMC.Networking.States;

public class StatusStateHandler : StateHandler
{
    GameServer server => manager.client.gameServer;
    public StatusStateHandler(PacketManager manager) : base(manager) { }


    public override void HandlePacket(IncomingPacket packet)
    {    Log.Verbose("Received status request!");
        switch (packet.packetId)
        {
            case 0x00:
            
                // StatusRequest so we send a status response
                manager.SendPacket(new StatusResponse(server.config.mc_version, server.config.protocol_version, server.motd));
                break;
            
            case 0x01:
                // StatusPing so we send a pong
                Log.Verbose("Ping!");
                manager.SendPacket(new StatusPongPacket(new StatusPingPacket(packet)));
                manager.client.Disconnect();
                break;
            default:
                break;
        }
    }
}