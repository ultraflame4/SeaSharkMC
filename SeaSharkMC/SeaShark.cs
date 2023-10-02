using SeaSharkMC.Networking;

namespace SeaSharkMC;

public class SeaShark
{
    private ClientsManager clientsManager;

    public SeaShark(ClientsManager clientsManager)
    {
        this.clientsManager = clientsManager;
    }


    public void Run()
    {
        clientsManager.Start();
    }
}