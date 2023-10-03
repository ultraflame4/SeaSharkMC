using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.old.Networking;
using Serilog;

namespace SeaSharkMC.Networking;

public class ClientHandler
{
    readonly TcpClient tcpClient;
    MinecraftNetworkClient mc;
    public readonly NetworkStream ns;
    readonly PacketManager packetManager;
    public readonly ILogger Log = Serilog.Log.ForContext<ClientHandler>();

    public ClientHandler(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
        ns = tcpClient.GetStream();
        packetManager = new PacketManager(this);
        mc = new MinecraftNetworkClient(this.tcpClient);
    }

    public IPEndPoint Endpoint => tcpClient.Client.LocalEndPoint as IPEndPoint ?? throw new NullReferenceException();
    public IPAddress Host => Endpoint.Address;
    public int Port => Endpoint.Port;

    public void Listen()
    {
        Log.Information($"Listening to {Host}");
        while (tcpClient.Connected)
        {
            try
            {
                if (ns.DataAvailable)
                {
                    IncomingPacket? incomingPacket = IncomingPacket.Read(ns);
                    if (incomingPacket == null) continue;
                    packetManager.Recieve(incomingPacket,mc);
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