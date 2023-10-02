using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Serilog;
using Serilog.Core;

namespace SeaSharkMC.Networking;

public class ClientsManager
{
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
    
    public ClientsManager(IPAddress host, int port)
    {
        server = new TcpListener(host, port);
        connectionThread = new Thread(ReceiveConnections);
    }

    public void Start()
    {
        logger.Information($"Starting server at {Host}:{Port}");
        server.Start();
        connectionThread.Start();
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
        ClientHandler client = new ClientHandler(tcpClient);
        logger.Information($"Accepted connection to {client.Host}:{client.Port}");
        client.Listen();
    }
}