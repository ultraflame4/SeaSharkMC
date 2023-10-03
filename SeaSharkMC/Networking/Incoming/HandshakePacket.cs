using System;
using System.IO;
using SeaSharkMC.Networking.Datatypes;
using SeaSharkMC.Networking.States;

namespace SeaSharkMC.Networking.Incoming;

public class HandshakePacket
{
    public IncomingPacket? packet { get; }
    public int protocolVersion { get; }
    public string serverAddress { get; }
    public ushort serverPort { get; }
    public ClientState nextState { get; }

    public HandshakePacket(IncomingPacket? packet)
    {
        this.packet = packet;
        protocolVersion = VarInt.ReadFrom(packet.data);
        serverAddress = VarIntString.ReadFrom(packet.data); 
        serverPort = packet.data.ReadUShort();
        nextState = (ClientState)(int)VarInt.ReadFrom(packet.data);
    }
    
}