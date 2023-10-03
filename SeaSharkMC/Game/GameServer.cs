using System.Collections.Generic;

namespace SeaSharkMC.Game;

public class GameServer
{
    List<Player> players = new();
    
    public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}