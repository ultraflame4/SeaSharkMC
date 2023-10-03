using System;
using System.IO;
using SeaSharkMC.Networking.Datatypes;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.Outgoing;
using SeaSharkMC.Networking.States;
using Serilog;
using ClientState = SeaSharkMC.Networking.States.ClientState;

namespace SeaSharkMC.Networking;

public class PacketManager
{
    public readonly ClientHandler client;
    public ClientState State { get; private set; }
    public KeepAliveHandler keepAliveHandler { get; }
    
    private StateHandler currentHandler;
    private HandshakeStateHandler handshakeStateHandler;
    private StatusStateHandler statusStateHandler;
    private LoginStateHandler loginStateHandler;
    private PlayStateHandler playStateHandler;
    private ILogger Log;

    public PacketManager(ClientHandler client)
    {
        keepAliveHandler = new KeepAliveHandler(this);
        Log = client.Log.ForContext(GetType());
        this.client = client;
        statusStateHandler = new(this);
        handshakeStateHandler = new(this);
        loginStateHandler = new(this);
        playStateHandler = new(this);

        State = ClientState.HANDSHAKE;
        currentHandler = handshakeStateHandler;
    }

    public void SwitchState(ClientState state)
    {
        State = state;
        currentHandler.StateExit();
        switch (state)
        {
            case ClientState.HANDSHAKE:
                currentHandler = handshakeStateHandler;
                break;
            case ClientState.STATUS:
                currentHandler = statusStateHandler;
                break;
            case ClientState.LOGIN:
                currentHandler = loginStateHandler;
                break;
            case ClientState.CONFIG:
                break;
            case ClientState.PLAY:
                currentHandler = playStateHandler;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

     
        currentHandler.StateEnter();
    }

    /// <summary>
    /// "Sorts" the packet and sends it to the correct handler
    /// </summary>
    /// <param name="packet"></param>
    public void Recieve(IncomingPacket packet)
    {
        try
        {
            // Log.Verbose("Handling packet {0}, state {1}", packet.packetId, State);
            currentHandler.HandlePacket(packet);
        }
        catch(Exception e)
        {
            Log.Error(e,
                "Error while handling packet!  Kicking client!!! " +
                "packet info- id: {0}, length: {2}, data size: {1} Client State {4}" +
                "\n===Hexdump start===\n{3}\n===Hexdump end===",
                packet.packetId, packet.data.Capacity,packet.length,packet.data.HexDump(),State);
            client.Disconnect();
        }
    }

    public void SendPacket(OutgoingPacket packet)
    {
        // Log.Verbose("Sending packet {0} of id {1}", packet.GetType().Name,packet.packetId);
        // var sample = new MemoryStream();
        // packet.Write(sample);
        // Log.Verbose("Sample Packet data: {0}", sample.HexDump());
        // Log.Verbose("Sample Packet length: {0} {1}", sample.Length, sample.Position);
        packet.Write(client.ns);
    }
}