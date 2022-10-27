﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.MinecraftPackets;
using Serilog;

namespace SeaSharkMC;

class Program
{
    static byte[] HelloMessage = Encoding.ASCII.GetBytes("Hello Visitor!");
    static ServerPacketsManager _serverPacketsManager;

    static void Main(string[] args)
    {
        Program main = new Program();
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext:l} | {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

        _serverPacketsManager = ServerPacketsManager.getInstance();

        main.server_start(); //starting the server99
        Console.WriteLine("Enter to exit");
        Console.ReadLine();
        Console.WriteLine("Exiting...");
        Log.CloseAndFlush();
    }

    TcpListener server = new TcpListener(IPAddress.Any, 9999);

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
        TcpClient client = server.EndAcceptTcpClient(result); //creates the TcpClient
        NetworkStream ns = client.GetStream();
        ns.Write(HelloMessage, 0, HelloMessage.Length);
        String ipAdrress = client.GetIpAddress();
        Log.Information($"Connected to {ipAdrress}");
        /* here you can add the code to send/receive data */
        Log.Debug($"Connection client buffer size: {client.ReceiveBufferSize}");
        while (true)
        {
            byte[] msg = new byte[1024]; //the messages arrive as byte array
            ns.Read(msg, 0, msg.Length); //the same networkstream reads the message sent by the client

            string message = Encoding.ASCII.GetString(msg).Trim('\0');

            if (message.Length == 0)
            {
                Log.Information($"{ipAdrress} Connection lost");
                break;
            }

            Log.Verbose($"NETWORK READ ::: From {ipAdrress} '{msg.ConvertBytesToHex()}' {message.Length}");
            try
            {
                _serverPacketsManager.RecieveRawNetworkBytes(msg, client);
            }
            catch (Exception e)
            {
                Log.Error(e,"An error has occured");
                ns.Close();
                client.Close();
                break;
            }
        }
    }
}