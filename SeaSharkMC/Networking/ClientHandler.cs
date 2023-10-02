﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.MinecraftPackets;
using Serilog;

namespace SeaSharkMC.Networking;

public class ClientHandler
{
    TcpClient tcpClient;
    MinecraftNetworkClient mc;
    readonly NetworkStream ns;
    readonly PacketManager packetManager = new PacketManager();
    readonly ILogger Log = Serilog.Log.ForContext<ClientHandler>();

    public ClientHandler(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
        ns = tcpClient.GetStream();
        mc = new MinecraftNetworkClient(this.tcpClient);
    }

    public IPEndPoint Endpoint => tcpClient.Client.LocalEndPoint as IPEndPoint ?? throw new NullReferenceException();
    public IPAddress Host => Endpoint.Address;
    public int Port => Endpoint.Port;

    public void Listen()
    {
        while (tcpClient.Connected)
        {
            try
            {
                if (ns.DataAvailable)
                {
                    IncomingPacket incomingPacket = IncomingPacket.Read(ns);
                    packetManager.Sort(incomingPacket,mc);
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
}