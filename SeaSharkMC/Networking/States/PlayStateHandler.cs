using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.States;

public class PlayStateHandler : StateHandler
{
    
    public PlayStateHandler(PacketManager manager) : base(manager) { }

    public override void HandlePacket(IncomingPacket packet)
    {
        
    }
}