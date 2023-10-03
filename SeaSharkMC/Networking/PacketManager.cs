using System;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.Outgoing;
using SeaSharkMC.Networking.States;
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


    public PacketManager(ClientHandler client)
    {
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
        currentHandler.HandlePacket(packet);
    }

    public void SendPacket(OutgoingPacket packet)
    {
        packet.Write(client.ns);
    }
}