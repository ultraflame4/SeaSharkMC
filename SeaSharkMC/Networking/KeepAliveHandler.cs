using System;
using System.IO;
using System.Net.Sockets;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.Outgoing;
using SeaSharkMC.Networking.States;

namespace SeaSharkMC.Networking;

public class KeepAliveHandler
{
    int keepAliveInterval = 5;
    DateTime lastKeepAlive;
    PacketManager packetManager;
    public KeepAliveHandler(PacketManager packetManager) { this.packetManager = packetManager; }

    public void SendKeepAlivePacket()
    {
        lastKeepAlive = DateTime.Now;


        try
        {
            if (packetManager.State != ClientState.PLAY)
            {
                SendEmptyPacket();
                
            }
            else
            {
                var packet = new KeepAlivePacket_C(lastKeepAlive.Ticks);
                packetManager.SendPacket(packet);
            }
        }
        catch (FailedToSendPacketException e)
        {
            packetManager.client.Log.Warning("Failed to send KeepAlive packet! Assume client disconnected!");
            packetManager.client.Disconnect();
        }
    }

    public void SendEmptyPacket() { packetManager.client.ns.WriteByte(0); }

    public void HandleKeepAlivePacket(IncomingPacket packet)
    {
        var keepAlivePacket = new KeepAlivePacket_S(packet);
        if (keepAlivePacket.keepAliveId == lastKeepAlive.Ticks)
        {
            packetManager.client.Log.Information("KeepAlive packet received!");
        }
        else
        {
            packetManager.client.Log.Warning(
                "KeepAlive packet received with wrong id! Will disconnect client! Expected {Expected} but got {Got}",
                lastKeepAlive.Ticks, keepAlivePacket.keepAliveId);
            packetManager.client.Disconnect();
        }
    }

    public void KeepAlive()
    {
        if (DateTime.Now - lastKeepAlive < TimeSpan.FromSeconds(keepAliveInterval)) return;
        SendKeepAlivePacket();
    }
}