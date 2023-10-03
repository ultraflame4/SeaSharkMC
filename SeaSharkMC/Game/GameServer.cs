using System;
using System.Collections.Generic;
using System.IO;
using SeaSharkMC.Networking;
using Serilog;
using SharpNBT;
using SharpNBT.SNBT;

namespace SeaSharkMC.Game;

public class GameServer
{
    private ILogger Log = Logging.Here<GameServer>();
    List<Player> players = new();
    public readonly ServerConfig config;
    public World defaultWorld;
    
    public GameServer()
    {
        defaultWorld = new World("minecraft:overworld");

        config = new ServerConfig(
            StringNbt.Parse(File.ReadAllText("./dimension_codec.snbt")),
            new [] {defaultWorld},
            defaultWorld
        );
    }

    public Player CreatePlayer(string username, ClientHandler clientHandler)
    {
        var p = new Player(username, this, Guid.NewGuid(), clientHandler,defaultWorld);
        p.abilities.allowFlight = true;
        p.abilities.flying = true;
        players.Add(p);
        return p;
    }
}