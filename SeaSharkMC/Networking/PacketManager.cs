using System;
using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.old.Networking;
using SeaSharkMC.Networking.States;
using SeaSharkMC.old.Networking.MinecraftPackets;
using ClientState = SeaSharkMC.Networking.States.ClientState;

namespace SeaSharkMC.Networking;

public class PacketManager
{
    private readonly ClientHandler clientHandler;
    public ClientState State { get; private set; } = ClientState.LOGIN;
    private StateHandler currentHandler;
    private HandshakeStateHandler handshakeState;
    private LoginStateHandler loginStateHandler;


    public PacketManager(ClientHandler clientHandler)
    {
        this.clientHandler = clientHandler;
        handshakeState = new(this);
        loginStateHandler = new(this);

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
    public void Recieve(IncomingPacket packet, MinecraftNetworkClient? sourceClient = null)
    {
        currentHandler.HandlePacket(packet);
        // todo change GenericMinecraftPacket to use the new IncomingPacket
        // todo remove sourceClient from GenericMinecraftPacket and use a different method to pass it. This is a temp solution
        // ServerPacketsManager.GetInstance().RecievePacketFrames(new GenericMinecraftPacket(packet.data,packet.packetId,packet.length,sourceClient));
    }
}