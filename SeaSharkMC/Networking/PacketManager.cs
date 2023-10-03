using System;
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
    private StateHandler currentHandler;
    private HandshakeStateHandler handshakeState;
    private LoginStateHandler loginStateHandler;
    private PlayStateHandler playStateHandler;
    private ILogger Log;

    public PacketManager(ClientHandler client)
    {
        Log = client.Log.ForContext(GetType());
        this.client = client;
        handshakeState = new(this);
        loginStateHandler = new(this);
        playStateHandler = new(this);

        State = ClientState.HANDSHAKE;
        currentHandler = handshakeState;
    }

    public void SwitchState(ClientState state)
    {
        State = state;
        currentHandler.StateExit();
        switch (state)
        {
            case ClientState.HANDSHAKE:
                currentHandler = handshakeState;
                break;
            case ClientState.STATUS:
                Log.Warning("Status state is not implemented yet! Switching back to handshake state!");
                SwitchState(ClientState.HANDSHAKE);
                break;
            case ClientState.LOGIN:
                currentHandler = loginStateHandler;
                break;
            case ClientState.CONFIG:
                break;
            case ClientState.PLAY:
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
            currentHandler.HandlePacket(packet);
        }
        catch(Exception e)
        {
            Log.Error(e,
                "Error while handling packet! Kicking client!!! " +
                "packet info- id: {0}, length: {2}, data size: {1}" +
                "\n===Hexdump start===\n{3}\n===Hexdump end===",
                packet.packetId, packet.data.Capacity,packet.length,packet.data.HexDump());
            client.Disconnect();
        }
    }

    public void SendPacket(OutgoingPacket packet)
    {
        packet.Write(client.ns);
    }
}