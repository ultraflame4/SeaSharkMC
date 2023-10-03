using SeaSharkMC.Networking.Incoming;

namespace SeaSharkMC.Networking.States;

public abstract class StateHandler
{
    protected readonly PacketManager manager;
    protected StateHandler(PacketManager manager)
    {
        this.manager = manager;
    }

    public virtual void StateEnter() { }
    public virtual void StateExit() { }
    public abstract void HandlePacket(IncomingPacket? packet);
}