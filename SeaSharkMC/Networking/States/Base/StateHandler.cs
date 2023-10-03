using SeaSharkMC.Networking.Incoming;
using Serilog;

namespace SeaSharkMC.Networking.States;

public abstract class StateHandler
{
    protected readonly PacketManager manager;
    protected readonly ILogger Log;
    protected StateHandler(PacketManager manager)
    {
        this.manager = manager;
        
        Log = Logging.Here(GetType()).ForContext("Prefix", $"({this.manager.clientHandler.Host}:{this.manager.clientHandler.Port}) ");
    }

    public virtual void StateEnter() { }
    public virtual void StateExit() { }
    public abstract void HandlePacket(IncomingPacket? packet);
}