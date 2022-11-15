using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using SeaSharkMC.Networking.MinecraftPackets;
using SeaSharkMC.Networking.MinecraftPackets.Client;
using Serilog;

namespace SeaSharkMC.Networking;

public class ServerPacketsManager : MarshalByRefObject
{
    private static ServerPacketsManager instance;
    private ILogger log;


    private ServerPacketsManager()
    {
        log = Log.Logger.ForContext<ServerPacketsManager>();
    }

    public static ServerPacketsManager GetInstance()
    {
        if (instance == null)
        {
            instance = new ServerPacketsManager();
        }

        return instance;
    }

    public void SendLoginSuccessPacket(String username, MinecraftNetworkClient client)
    {
        byte[] uuid = GeneralUtils.GetUUId();
        LoginSuccessPacket packet = new LoginSuccessPacket(uuid, username);
        byte[] bytes = packet.ToBytesArray();
        log.Information($"Player {username} logged in successfully from {client.IpAddress}");
        client.Ns.Write(packet.ToBytesArray(),0,bytes.Length);
    }
    public void ReceiveHandshakePackets(RawMinecraftPacket packet)
    {
        switch (packet.SourceClient.state)
        {
            case ClientState.NONE:
                HandshakePacket handshakePacket = new HandshakePacket(packet);
                log.Verbose($"Server Handshake with {packet.SourceClient.IpAddress}; " +
                            $"PacketId: {handshakePacket.PacketId}, " +
                            $"ProtocolVersion: {handshakePacket.ProtocolVersion} " +
                            $"ServerAddress: {handshakePacket.ServerAddress} " +
                            $"ServerPort: {handshakePacket.ServerPort} " +
                            $"NextState: {handshakePacket.NextState}");

                packet.SourceClient.state = handshakePacket.NextState;
                break;

            case ClientState.STATUS:
                break;
            
            case ClientState.LOGIN:
                LoginStartPacket loginStartPacket = new LoginStartPacket(packet);
                // todo maybe add in encryption for online mode
                log.Information($"Player {loginStartPacket.PlayerUsername} attempting to connect from {packet.SourceClient.IpAddress}");
                SendLoginSuccessPacket(loginStartPacket.PlayerUsername, packet.SourceClient);
                break;
            
            default:
                break;
        }

        return;
    }

    public void RecievePacketFrames(RawMinecraftPacket packet)
    {
        // aSize, bSize, .. are the size of the var Int

        log.Verbose($"SERVER DECODE: PacketId: {packet.PacketId} PacketLength: {packet.PacketLength}");
        
        switch (packet.PacketId)
        {
            case 0x00:
                ReceiveHandshakePackets(packet);
                break;

            default:
                break;
        }
    }
    public void RecieveRawNetworkBytes(byte[] byteArray, MinecraftNetworkClient client)
    {
        foreach (var packetFrame in RawMinecraftPacket.Create(byteArray,client))
        {
            RecievePacketFrames(packetFrame);
        }
    }
}