using System;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.Outgoing;

namespace SeaSharkMC.Networking;

public class KeepAliveHandler
{
    int keepAliveInterval = 19;
    DateTime lastKeepAlive;
    PacketManager packetManager;
    public KeepAliveHandler(PacketManager packetManager) { this.packetManager = packetManager; }
    
    public void SendKeepAlivePacket()
    {
        
        lastKeepAlive = DateTime.Now;
        packetManager.client.Log.Information("KeepAlive packet send with id {id}", lastKeepAlive.Ticks);
        var packet = new KeepAlivePacket_C(lastKeepAlive.Ticks);
        packetManager.SendPacket(packet);
    }

    public void HandleKeepAlivePacket(IncomingPacket packet)
    {
        var keepAlivePacket = new KeepAlivePacket_S(packet);
        if (keepAlivePacket.keepAliveId == lastKeepAlive.Ticks)
        {
            packetManager.client.Log.Information("KeepAlive packet received!");
        }
        else
        {
            packetManager.client.Log.Warning("KeepAlive packet received with wrong id! Will disconnect client!");
            packetManager.client.Disconnect();
        }
    }

    public void KeepAlive()
    {
        if (DateTime.Now - lastKeepAlive > TimeSpan.FromSeconds(keepAliveInterval))
        {
            SendKeepAlivePacket();
        }
    }
}