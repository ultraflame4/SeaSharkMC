using System;
using SeaSharkMC.Game;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.Outgoing;

namespace SeaSharkMC.Networking.States;

public class LoginStateHandler : StateHandler
{
    public LoginStateHandler(PacketManager manager) : base(manager) { }


    public void OnLoginStart(LoginStartPacket packet)
    {
        Log.Information("Player {Username} attempting login!", packet.playerUsername);
        var uuid = GeneralUtils.GetUUId();
        manager.SendPacket(new LoginSuccessPacket(packet.playerUsername, uuid));
        Log.Information("Player {Username} login success!", packet.playerUsername);
        manager.client.Player = new Player(packet.playerUsername, Guid.NewGuid(), manager.client);
        manager.client.gameServer.AddPlayer(manager.client.Player);
        manager.SwitchState(ClientState.PLAY);
    }

    public override void HandlePacket(IncomingPacket? packet)
    {
        switch (packet.packetId)
        {
            case 0:
                OnLoginStart(new LoginStartPacket(packet));
                break;
            default:
                Log.Warning(
                    "Recieved unknown packet id {PacketId} in state {State}! Will kick client!",
                    packet.packetId, manager.State);
                manager.client.Disconnect();
                return;
        }
    }
}