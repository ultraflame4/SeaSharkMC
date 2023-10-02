using SeaSharkMC.Networking.Incoming;
using SeaSharkMC.Networking.MinecraftPackets;

namespace SeaSharkMC.Networking;

public class PacketManager
{
    /// <summary>
    /// "Sorts" the packet and sends it to the correct handler
    /// </summary>
    /// <param name="packet"></param>
    public void Sort(IncomingPacket packet, MinecraftNetworkClient? sourceClient = null)
    {
        // todo change GenericMinecraftPacket to use the new IncomingPacket
        ServerPacketsManager.GetInstance().RecievePacketFrames(new GenericMinecraftPacket(packet.data,packet.packetId,packet.length,sourceClient));
    }
}