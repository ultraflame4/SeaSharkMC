using System;
using System.Text;
using Serilog;

namespace SeaSharkMC.Networking.MinecraftPackets;

public class HandshakePacket : MinecraftServerPacket
{
    protected int protocolVersion;
    protected string serverAddress;
    protected ushort serverPort;
    protected ClientState nextState;

    public int ProtocolVersion => protocolVersion;

    public string ServerAddress => serverAddress;

    public int ServerPort => serverPort;

    public ClientState NextState => nextState;

    public HandshakePacket(byte[] bytesArray) : base(bytesArray)
    {
        packetId = 0;

        (protocolVersion, int protocolSize) = PacketDataUtils.ReadVarInt(bytesArray, packetDataOffset);
        
        (serverAddress, int serverAddrSize) = PacketDataUtils.ReadVarIntString(bytesArray, packetDataOffset + protocolSize);

        int serverPortOffset = packetDataOffset  + protocolSize + serverAddrSize;
        serverPort = (ushort)((ReadDataByte(serverPortOffset) << 8) | ReadDataByte(serverPortOffset + 1));

        (int nextStateNumber, int nxtStateSize) = PacketDataUtils.ReadVarInt(bytesArray, serverPortOffset + 2);
        nextState = (ClientState)nextStateNumber;
    }
    
}
