using System;
using System.Text;
using Serilog;

namespace SeaSharkMC.Networking.MinecraftPackets;

public class HandshakePacket : MinecraftBasePacket
{
    protected int protocolVersion;
    protected string serverAddress;
    protected ushort serverPort;
    protected ClientState nextState;

    public int ProtocolVersion => protocolVersion;

    public string ServerAddress => serverAddress;

    public int ServerPort => serverPort;

    public ClientState NextState => nextState;

    public HandshakePacket(MinecraftPacketFrame packetFrame) : base(packetFrame)
    {
        packetId = 0;

        (protocolVersion, int protocolSize) = PacketDataUtils.ReadVarInt(bytesArray, packetFrame.PacketDataOffset);
        
        (serverAddress, int serverAddrSize) = PacketDataUtils.ReadVarIntString(bytesArray, packetFrame.PacketDataOffset + protocolSize);

        int serverPortOffset = packetFrame.PacketDataOffset  + protocolSize + serverAddrSize;
        serverPort = (ushort)((ReadDataByte(serverPortOffset) << 8) | ReadDataByte(serverPortOffset + 1));

        (int nextStateNumber, int nxtStateSize) = PacketDataUtils.ReadVarInt(bytesArray, serverPortOffset + 2);
        nextState = (ClientState)nextStateNumber;
    }

    protected override byte[] GetDataByteArray()
    {
        throw new NotImplementedException();
    }
}
