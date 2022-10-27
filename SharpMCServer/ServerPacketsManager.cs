using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Serilog;
using SharpMCServer.MinecraftPackets;

namespace SharpMCServer;

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

    public void DecodeLogin(int dataOffset, int dataLength, byte[] bytesArray, TcpClient client)
    {
        HandshakePacket handshakePacket = new HandshakePacket(bytesArray);
        log.Verbose($"Server Handshake with {client.GetIpAddress()}; " +
                    $"PacketId: {handshakePacket.PacketId}, " +
                    $"ProtocolVersion: {handshakePacket.ProtocolVersion} " +
                    $"ServerAddress: {handshakePacket.ServerAddress} " +
                    $"ServerPort: {handshakePacket.ServerPort} " +
                    $"NextState: {handshakePacket.NextState}");

        return;
    }

    public void DecodeRawNetworkBytes(byte[] byteArray, TcpClient client)
    {
        // aSize, bSize, .. are the size of the var Int
        (int PacketLength, int aSize) = PacketDataUtils.ReadVarInt(byteArray);

        (int PacketId, int bSize) = PacketDataUtils.ReadVarInt(byteArray, aSize);

        int dataOffset = aSize + bSize;
        int dataLength = PacketLength - bSize;

        log.Verbose($"SERVER DECODE: PacketId: {PacketId} PacketLength: {PacketLength} Sizes {aSize} {bSize}");
        
        switch (PacketId)
        {
            case 0:
                DecodeLogin(dataOffset, dataLength, byteArray, client);
                break;

            default:
                break;
        }
    }
}