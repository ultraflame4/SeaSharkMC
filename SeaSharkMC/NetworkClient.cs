using System.Net.Sockets;
using SeaSharkMC.MinecraftPackets;

namespace SeaSharkMC;

public class NetworkClient
{
    private string ipAddress;
    private TcpClient tcpClient;
    private NetworkStream ns;

    public int state = 0;
    public string IpAddress => ipAddress;
    public NetworkStream Ns => ns;

    public NetworkClient(TcpClient tcpClient)
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