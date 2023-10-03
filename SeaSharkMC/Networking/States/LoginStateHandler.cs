﻿using SeaSharkMC.Networking.Incoming;
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
                manager.clientHandler.Disconnect();
                return;
        }
    }
}