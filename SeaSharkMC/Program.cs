using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Networking;
using SeaSharkMC.Networking.MinecraftPackets;
using Serilog;

namespace SeaSharkMC;
class Program
{
    static ServerPacketsManager _serverPacketsManager;

    TcpListener server = new TcpListener(IPAddress.Any, 9999);

    static void Main(string[] args)
    {
        Program main = new Program();
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext:l} | {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

        _serverPacketsManager = ServerPacketsManager.GetInstance();

        main.server_start(); //starting the server99
        var endpoint = main.server.LocalEndpoint as IPEndPoint;
        Console.WriteLine($"Started server at {endpoint?.Address}:{endpoint?.Port}");
        Console.WriteLine("Enter to exit");
        Console.ReadLine();
        Console.WriteLine("Exiting...");
        Log.CloseAndFlush();
    }

    private void server_start()
    {
        server.Start();
        accept_connection(); //accepts incoming connections
    }

    private void accept_connection()
    {
        server.BeginAcceptTcpClient(handle_connection, server); //this is called asynchronously and will run in a different thread
    }

    private void handle_connection(IAsyncResult result) //the parameter is a delegate, used to communicate between threads
    {
        accept_connection(); //once again, checking for any other incoming connections
        TcpClient tcpClient = server.EndAcceptTcpClient(result); //creates the TcpClient
        MinecraftNetworkClient client = new MinecraftNetworkClient(tcpClient);
        
        Log.Information($"Connected to {client.IpAddress}");

        while (true)
        {
            byte[] msg = new byte[1024]; //the messages arrive as byte array
            client.Ns.Read(msg, 0, msg.Length); //the same networkstream reads the message sent by the client

            string message = Encoding.ASCII.GetString(msg).Trim('\0');

            if (message.Length == 0)
            {
                break;
            }

            Log.Verbose($"NETWORK READ ::: From {client.IpAddress} '{msg.ConvertBytesToHex()}' {message.Length}");
            try
            {
                _serverPacketsManager.RecieveRawNetworkBytes(msg, client);
            }
            catch (Exception e)
            {
                Log.Error(e,"An error has occured");
                client.Close();
                break;
            }
        }
        Log.Information($"{client.IpAddress} Connection lost");
        
    }
}