using System.Net.Sockets;
using SeaSharkMC.Networking.MinecraftPackets;

namespace SeaSharkMC.Networking;

public class MinecraftNetworkClient
{
    private string ipAddress;
    private TcpClient tcpClient;
    private NetworkStream ns;

    public ClientState state = ClientState.NONE;
    public string IpAddress => ipAddress;
    public NetworkStream Ns => ns;

    public MinecraftNetworkClient(TcpClient tcpClient)
    {
        ipAddress = tcpClient.GetIpAddress();
        ns = tcpClient.GetStream();
        this.tcpClient = tcpClient;
    }

    public void Close()
    {
        ns.Close();
        tcpClient.Close();
    }
}