using System;
using System.Net.Sockets;
using SeaSharkMC.old.Networking.MinecraftPackets;

namespace SeaSharkMC.old.Networking;

public class MinecraftNetworkClient
{
    private string ipAddress;
    private TcpClient tcpClient;
    private NetworkStream ns;

    public ClientState state = ClientState.NONE;
    public event Action ClientDisconnect;
    public string IpAddress => ipAddress;
    public NetworkStream Ns => ns;

    public MinecraftNetworkClient(TcpClient tcpClient)
    {
        ipAddress = tcpClient.GetIpAddress();
        ns = tcpClient.GetStream();
        this.tcpClient = tcpClient;
    }

    public void SendPacket(MinecraftBasePacket packet)
    {
        byte[] bytes = packet.ToBytesArray();
        ns.Write(packet.ToBytesArray(), 0, bytes.Length);
    }

    public void Close()
    {
        ns.Close();
        tcpClient.Close();
        ClientDisconnect?.Invoke();
    }
}