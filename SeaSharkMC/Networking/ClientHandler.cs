using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Game;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.old.Networking;
using Serilog;

namespace SeaSharkMC.Networking;

public class ClientHandler
{
    public readonly GameServer gameServer;
    public readonly NetworkStream ns;
    public Player? Player;
    readonly TcpClient tcpClient;
    
    readonly PacketManager packetManager;
    public readonly ILogger Log ;

    public ClientHandler(TcpClient tcpClient, GameServer gameServer)
    {
        this.tcpClient = tcpClient;
        this.gameServer = gameServer;
        ns = tcpClient.GetStream();
        packetManager = new PacketManager(this);
        Log = Logging.Here<ClientHandler>().ForContext("Prefix", $"({Host}:{Port}) ");
        
    }

    public IPEndPoint Endpoint => tcpClient.Client.LocalEndPoint as IPEndPoint ?? throw new NullReferenceException();
    public IPAddress Host => Endpoint.Address;
    public int Port => Endpoint.Port;

    public void Listen()
    {
        Log.Verbose("Listening to {0}",Host);
        while (tcpClient.Connected)
        {
            try
            {
                if (ns.DataAvailable)
                {
                    IncomingPacket? incomingPacket = IncomingPacket.Read(ns);
                    if (incomingPacket == null) continue;
                    packetManager.Recieve(incomingPacket);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "An error has occured");
                ns.Close();
                tcpClient.Close();
                break;
            }
        }

        Log.Information($"{Host} Connection lost");
    }

    public void Disconnect()
    {
        ns.Close();
        tcpClient.Close();
    }
}