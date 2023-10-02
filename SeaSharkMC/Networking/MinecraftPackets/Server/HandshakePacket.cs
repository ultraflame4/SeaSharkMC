using System;
using System.IO;
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

    public HandshakePacket( int protocolVersion, string serverAddress, ushort serverPort, ClientState nextState) : base(0x00)
    {
        this.protocolVersion = protocolVersion;
        this.serverAddress = serverAddress;
        this.serverPort = serverPort;
        this.nextState = nextState;
    }

    public HandshakePacket(GenericMinecraftPacket packet) : base(packet)
    {
        packetId = 0;

        protocolVersion = packet.Stream.ReadVarInt();
        serverAddress = packet.Stream.ReadVarIntString();
        serverPort = (ushort)((packet.Stream.ReadByte() << 8) | packet.Stream.ReadByte());
        nextState = (ClientState)packet.Stream.ReadVarInt();
    }

    protected override void OnDataToBytes(MemoryStream dataStream)
    {
        throw new NotImplementedException();
    }
}
