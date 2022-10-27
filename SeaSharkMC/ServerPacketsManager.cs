using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.MinecraftPackets;
using SeaSharkMC.MinecraftPackets.Client;
using Serilog;

namespace SeaSharkMC;

public class ServerPacketsManager : MarshalByRefObject
{
    private static ServerPacketsManager instance;
    private ILogger log;


    private ServerPacketsManager()
    {
        log = Log.Logger.ForContext<ServerPacketsManager>();
    }

    public static ServerPacketsManager getInstance()
    {
        if (instance == null)
        {
            instance = new ServerPacketsManager();
        }

        return instance;
    }

    public void SendLoginSuccessPacket(String username, NetworkClient client)
    {
        byte[] uuid = GeneralUtils.GetUUId();
        LoginSuccessPacket packet = new LoginSuccessPacket(uuid, username);
        byte[] bytes = packet.ToBytesArray();
        Log.Debug($"SUCCESS LOGIN {packet.ToBytesArray().ConvertBytesToHex()}");
        client.Ns.Write(packet.ToBytesArray(),0,bytes.Length);
    }
    public void ReceiveHandshakeLogin(MinecraftPacketFrame packetFrame)
    {
        switch (packetFrame.SourceClient.state)
        {
            case 0:
                HandshakePacket handshakePacket = new HandshakePacket(packetFrame.BytesArray);
                log.Verbose($"Server Handshake with {packetFrame.SourceClient.IpAddress}; " +
                            $"PacketId: {handshakePacket.PacketId}, " +
                            $"ProtocolVersion: {handshakePacket.ProtocolVersion} " +
                            $"ServerAddress: {handshakePacket.ServerAddress} " +
                            $"ServerPort: {handshakePacket.ServerPort} " +
                            $"NextState: {handshakePacket.NextState}");

                packetFrame.SourceClient.state = handshakePacket.NextState;
                break;

            case 1:
                break;
            
            case 2:
                LoginStartPacket loginStartPacket = new LoginStartPacket(packetFrame.BytesArray);
                log.Information($"Player {loginStartPacket.PlayerUsername} has logged in from {packetFrame.SourceClient.IpAddress}");
                // todo maybe add in encryption for online mode
                SendLoginSuccessPacket(loginStartPacket.PlayerUsername, packetFrame.SourceClient);
                break;
            
            default:
                break;
        }

        return;
    }

    public void RecievePacketFrames(MinecraftPacketFrame packetFrame)
    {
        // aSize, bSize, .. are the size of the var Int

        log.Verbose($"SERVER DECODE: PacketId: {packetFrame.PacketId} PacketLength: {packetFrame.PacketLength}");
        
        switch (packetFrame.PacketId)
        {
            case 0:
                ReceiveHandshakeLogin(packetFrame);
                break;

            default:
                break;
        }
    }
    public void RecieveRawNetworkBytes(byte[] byteArray, NetworkClient client)
    {
        foreach (var packetFrame in MinecraftPacketFrame.Create(byteArray,client))
        {
            RecievePacketFrames(packetFrame);
        }
    }
}