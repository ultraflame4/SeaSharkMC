using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.old.Networking;
using SeaSharkMC.old.Networking.MinecraftPackets;

namespace SeaSharkMC.Networking;

public class PacketManager
{
    private readonly ClientHandler clientHandler;
    public PacketManager(ClientHandler clientHandler) { this.clientHandler = clientHandler; }

    /// <summary>
    /// "Sorts" the packet and sends it to the correct handler
    /// </summary>
    /// <param name="packet"></param>
    public void Sort(IncomingPacket packet, MinecraftNetworkClient? sourceClient = null)
    {
        // todo change GenericMinecraftPacket to use the new IncomingPacket
        // todo remove sourceClient from GenericMinecraftPacket and use a different method to pass it. This is a temp solution
        ServerPacketsManager.GetInstance().RecievePacketFrames(new GenericMinecraftPacket(packet.data,packet.packetId,packet.length,sourceClient));
    }
}