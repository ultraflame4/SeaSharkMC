using System;
using Abacus.DoublePrecision;
using SeaSharkMC.Networking;

namespace SeaSharkMC.Game;

public class Player
{
    public int entity_id = 0;
    public readonly Guid uuid;
    public readonly string Username;
    public readonly GameServer server;
    public readonly ClientHandler client;
    public Gamemode gamemode = Gamemode.CREATIVE;
    public Vector3 position = Vector3.Zero;
    /// <summary>
    /// World the player is in
    /// </summary>
    public World world;
    public PlayerAbilities abilities = new();
    public PlayerInventory inventory = new();
    
    public Player(string username, GameServer server, Guid uuid, ClientHandler client, World world)
    {
        this.uuid = uuid;
        Username = username;
        this.server = server;
        this.client = client;
        this.world = world;
    }
    
}