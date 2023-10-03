using System;
using SeaSharkMC.Networking;

namespace SeaSharkMC.Game;

public class Player
{
    public readonly Guid uuid;
    public readonly string Username;
    public readonly ClientHandler client;
    
    public Player(string username, Guid uuid, ClientHandler client)
    {
        this.uuid = uuid;
        Username = username;
        this.client = client;
    }
    
    public static Player Create(string username, ClientHandler client)
        => new(username, Guid.NewGuid(), client);
}