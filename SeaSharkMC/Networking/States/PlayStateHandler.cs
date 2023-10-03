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
        // refer to https://wiki.vg/index.php?title=Protocol_FAQ&diff=17474&oldid=17440#What.27s_the_normal_login_sequence_for_a_client.3F
        
        player = manager.client.Player ?? throw new NullReferenceException("Error: Player is null!");
        
        manager.SendPacket(new JoinGamePacket(player.server.config,player.entity_id,false,(byte)player.gamemode));
        manager.SendPacket(new PluginMessagePacket_C("minecraft:brand","SeaSharkMC"));
        manager.SendPacket(new GameDifficultyPacket(player.world.difficulty,player.world.difficultyLocked));
        
    }

    public override void HandlePacket(IncomingPacket packet)
    {
        
    }
}