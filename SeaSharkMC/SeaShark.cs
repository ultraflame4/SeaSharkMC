using System.Net;
using SeaSharkMC.Game;
using SeaSharkMC.Networking;

namespace SeaSharkMC;

public class SeaShark
{
    private ClientsManager clientsManager;
    private GameServer gameServer;

    public SeaShark(IPAddress host, int port)
    {
        this.gameServer = new ();
        clientsManager = new ClientsManager(host, port,gameServer);
    }


    public void Run()
    {
        clientsManager.Start();
    }
}