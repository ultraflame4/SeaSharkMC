using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SeaSharkMC.Game;
using Serilog;
using Serilog.Core;

namespace SeaSharkMC.Networking;

public class ClientsManager
{
    private readonly GameServer gameServer;
    private TcpListener server;
    private ILogger logger = Log.ForContext<ClientsManager>();
    public IPEndPoint Endpoint => server.LocalEndpoint as IPEndPoint?? throw new NullReferenceException();
    public IPAddress Host => Endpoint.Address;
    public int Port => Endpoint.Port;

    /// <summary>
    /// Whether the server is accepting connections
    /// </summary>
    public bool acceptingConnections = true;
    /// <summary>
    /// Thread that accepts connections
    /// </summary>
    public Thread connectionThread { get; private set; } 
    
    public ClientsManager(IPAddress host, int port, GameServer gameServer)
    {
        this.gameServer = gameServer;
        server = new TcpListener(host, port);
        connectionThread = new Thread(ReceiveConnections);
    }

    public void Start()
    {
        logger.Information("Starting server at {0}:{1}",Host,Port);
        server.Start();
        connectionThread.Start();
    }

    public void Stop()
    {
        logger.Information("Stopping server...");
        acceptingConnections = false;
    }
    
    public void ReceiveConnections()
    {
        while (acceptingConnections)
        {
            server.BeginAcceptTcpClient(AcceptConnectionCallback, null);
            Thread.Sleep(100);
        }
    }
    
    
    private void AcceptConnectionCallback(IAsyncResult result)
    {
        TcpClient tcpClient = server.EndAcceptTcpClient(result);
        ClientHandler client = new ClientHandler(tcpClient,gameServer);
        logger.Information("Accepted connection from {0}",client.Endpoint);
        client.Listen();
    }
}