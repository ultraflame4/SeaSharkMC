using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.States;

public class LoginStateHandler : StateHandler
{
    public LoginStateHandler(PacketManager manager) : base(manager) { }

    public override void  HandlePacket(IncomingPacket packet)
    {
        
    }
}