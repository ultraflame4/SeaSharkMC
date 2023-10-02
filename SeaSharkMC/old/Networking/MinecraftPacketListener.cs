using System;
using SeaSharkMC.Networking.MinecraftPackets;
using Serilog;

namespace SeaSharkMC.Networking;


public abstract class MinecraftPacketListener
{
    public readonly int targetPacketId;
    protected ILogger logs = null;

    public MinecraftPacketListener(int targetPacketId)
    {
        this.targetPacketId = targetPacketId;
        logs = Log.ForContext(this.GetType());
    }
    
    

    /// <summary>
    /// Called when the server receives a packet with a packet id equal to targetPacketId
    /// </summary>
    /// <param name="packet">The packet frame. This contains useful information and methids such as the data offset and ReadData(x), things generally needed./param>
    public abstract void RecievePacketFrame(GenericMinecraftPacket packet);

    
    
}

