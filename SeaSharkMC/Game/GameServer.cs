using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using SharpNBT;
using SharpNBT.SNBT;

namespace SeaSharkMC.Game;

public class GameServer
{
    private ILogger Log = Logging.Here<GameServer>();
    List<Player> players = new();
    CompoundTag dimension_codec;

    public GameServer()
    {
        dimension_codec = StringNbt.Parse(File.ReadAllText("./dimension_codec.snbt"));
        
    }

    public void AddPlayer(Player player) { players.Add(player); }
}