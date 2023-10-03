using System;
using SeaSharkMC.Game;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.Outgoing;

namespace SeaSharkMC.Networking.States;

public class PlayStateHandler : StateHandler
{
    
    Player player;
    public PlayStateHandler(PacketManager manager) : base(manager) { }


    public override void StateEnter()
    {
        player = manager.client.Player ?? throw new NullReferenceException("Error: Player is null!");
        var joinGamePacket = new JoinGamePacket(player.server.config,player.entity_id,false,(byte)player.gamemode);
        Log.Verbose("Sending JoinGamePacket to {0}",player.Username);
        manager.SendPacket(joinGamePacket);
    }

    public override void HandlePacket(IncomingPacket packet)
    {
        
    }
}