using System;
using SeaSharkMC.Networking.MinecraftPackets;

namespace SeaSharkMC.Networking;

/// <summary>
/// All child class of this class is responsible for converting bytes to an instance of 'T' and vice versa. 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MinecraftPacketsListener<T> where T : MinecraftBasePacket
{
    public readonly int targetPacketId;
    
    protected MinecraftPacketsListener(int targetPacketId)
    {
        this.targetPacketId = targetPacketId;
    }

    /// <summary>
    /// Called when the server receives a packet with a packet id equal to targetPacketId
    /// </summary>
    /// <param name="packet">The packet frame. This contains useful information and methids such as the data offset and ReadData(x), things generally needed./param>
    public abstract void RecievePacketFrame(RawMinecraftPacket packet);

    
    
}

