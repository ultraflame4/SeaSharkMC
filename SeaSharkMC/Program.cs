using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Networking;
using Serilog;

namespace SeaSharkMC;
class Program
{
    static void Main(string[] args)
    {
        Program main = new Program();
        Log.Logger = Logging.Config().CreateLogger();

        
        var shark = new SeaShark(new ClientsManager(IPAddress.Any, 9999));
        shark.Run();
        
        Console.WriteLine("Enter to exit");
        Console.ReadLine();
        Console.WriteLine("Exiting...");
        Log.CloseAndFlush();
    }

    //
    // private void handle_connection(IAsyncResult result) //the parameter is a delegate, used to communicate between threads
    // {
    //     accept_connection(); //once again, checking for any other incoming connections
    //     TcpClient tcpClient = server.EndAcceptTcpClient(result); //creates the TcpClient
    //     MinecraftNetworkClient client = new MinecraftNetworkClient(tcpClient);
    //     
    //     Log.Information($"Connected to {client.IpAddress}");
    //
    //     while (true)
    //     {
    //         byte[] msg = new byte[1024]; //the messages arrive as byte array
    //         client.Ns.Read(msg, 0, msg.Length); //the same networkstream reads the message sent by the client
    //
    //         string message = Encoding.ASCII.GetString(msg).Trim('\0');
    //
    //         if (message.Length == 0)
    //         {
    //             break;
    //         }
    //
    //         Log.Verbose($"NETWORK READ ::: From {client.IpAddress} '{msg.ConvertBytesToHex()}' {message.Length}");
    //         try
    //         {
    //             _serverPacketsManager.RecieveRawNetworkBytes(msg, client);
    //         }
    //         catch (Exception e)
    //         {
    //             Log.Error(e,"An error has occured");
    //             client.Close();
    //             break;
    //         }
    //     }
    //     Log.Information($"{client.IpAddress} Connection lost");
    //     
    // }
}