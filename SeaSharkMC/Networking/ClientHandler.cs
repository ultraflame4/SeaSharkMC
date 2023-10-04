using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Game;
using SeaSharkMC.Networking.Incoming;

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

    public IPEndPoint Endpoint;
    public IPAddress Host => Endpoint.Address;
    public int Port => Endpoint.Port;
    public ClientHandler(TcpClient tcpClient, GameServer gameServer)
    {
        this.tcpClient = tcpClient;
        this.gameServer = gameServer;
        ns = tcpClient.GetStream();
        Endpoint = tcpClient.Client.RemoteEndPoint as IPEndPoint ?? throw new NullReferenceException("Cannot get ip endpoint!");
        Log = Logging.Here<ClientHandler>().ForContext("Prefix", $"({Host}:{Port}) ");
        packetManager = new PacketManager(this);
    }



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
                else
                {
                    packetManager.keepAliveHandler.KeepAlive();
                }
            }
            catch (FailedToSendPacketException e)
            {
                Log.Error(e, "Failed to send packet of type {type} and id {id}. Will disconnect!\n" +
                             "===Packet Hex Dump Start===\n{hexDump}\n===Packet Hex Dump End===",
                    e.packetType, e.packetId, e.hexData);
                if (e.innerException != null) Log.Error(e.innerException, "Inner exception");
                
                Disconnect();
            }
            catch (Exception e)
            {
                Log.Error(e, "An error has occured");
                Disconnect();
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