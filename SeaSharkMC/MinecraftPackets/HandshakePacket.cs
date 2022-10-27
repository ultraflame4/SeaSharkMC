using System;
using System.Text;
using Serilog;

namespace SeaSharkMC.MinecraftPackets;

public class HandshakePacket : MinecraftBasePacket
{
    protected int protocolVersion;
    protected string serverAddress;
    protected ushort serverPort;
    protected int nextState;

    public int ProtocolVersion => protocolVersion;

    public string ServerAddress => serverAddress;

    public int ServerPort => serverPort;

    public int NextState => nextState;

    public HandshakePacket(byte[] bytesArray) : base(bytesArray)
    {
        packetId = 0;

        (protocolVersion, int protocolSize) = PacketDataUtils.ReadVarInt(bytesArray, packetDataOffset);

        // Get server address sent from client
        (int serverAddressLength, int serverAddrLengthSize) = PacketDataUtils.ReadVarInt(bytesArray, packetDataOffset + protocolSize); // 1 get address size

        serverAddress = Encoding.UTF8.GetString(bytesArray, packetDataOffset + protocolSize + serverAddrLengthSize, serverAddressLength);

        int serverPortOffset = packetDataOffset  + protocolSize + serverAddrLengthSize + serverAddressLength;
        serverPort = (ushort)((ReadDataByte(serverPortOffset) << 8) | ReadDataByte(serverPortOffset + 1));

        (nextState, int nxtStateSize) = PacketDataUtils.ReadVarInt(bytesArray, serverPortOffset + 2);
    }
}
