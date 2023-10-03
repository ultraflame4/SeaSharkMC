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

    public GameServer()
    {
        var worlds = new World[1] {
                new World("minecraft:overworld")
        };

        config = new ServerConfig(
            StringNbt.Parse(File.ReadAllText("./dimension_codec.snbt")),
            worlds,
            worlds[0]
        );
    }

    public Player CreatePlayer(string username, ClientHandler clientHandler)
    {
        var p = new Player(username, this, Guid.NewGuid(), clientHandler);
        players.Add(p);
        return p;
    }
}