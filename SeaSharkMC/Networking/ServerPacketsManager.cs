using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Networking.MinecraftPackets;
using SeaSharkMC.Networking.MinecraftPackets.Client;
using SeaSharkMC.Networking.MinecraftPackets.Listeners;
using Serilog;

namespace SeaSharkMC.Networking;

public class ServerPacketsManager : MarshalByRefObject
{
    private static ServerPacketsManager instance;
    private ILogger log;
    
    private List<MinecraftPacketListener> packetListeners = new();

    private ServerPacketsManager()
    {
        log = Log.Logger.ForContext<ServerPacketsManager>();
        
        packetListeners.Add(new PlayerLoginPacketListener());
    }

    public static ServerPacketsManager getInstance()
    {
        if (instance == null)
        {
            instance = new ServerPacketsManager();
        }

        return instance;
    }
    public void RecievePacketFrames(MinecraftPacketFrame packetFrame)
    {
        // aSize, bSize, .. are the size of the var Int

        log.Verbose($"SERVER DECODE: PacketId: {packetFrame.PacketId} PacketLength: {packetFrame.PacketLength}");
        foreach (var listener in packetListeners)
        {
            if (listener.targetPacketId == packetFrame.PacketId)
            {
                listener.RecievePacketFrame(packetFrame);
                break;
            }
        }
    }

    public void RecieveRawNetworkBytes(byte[] byteArray, MinecraftNetworkClient client)
    {
        foreach (var packetFrame in MinecraftPacketFrame.Create(byteArray, client))
        {
            RecievePacketFrames(packetFrame);
        }
    }
}