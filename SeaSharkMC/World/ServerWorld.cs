using System.Collections.Generic;
using SeaSharkMC.Networking;
using SeaSharkMC.Networking.MinecraftPackets;
using Serilog;

namespace SeaSharkMC.World;

public class ServerWorld
{
    private static ServerWorld? instance;
    public readonly List<MinecraftPlayer> players = new List<MinecraftPlayer>();
    private ILogger logger;

    public ServerWorld()
    {
        logger = Log.ForContext<ServerWorld>();
    }
    public static ServerWorld getInstance()
    {
        if (instance == null)
        {
            instance = new ServerWorld();
        }

        return instance;
    }

    public MinecraftPlayer CreatePlayer(string username, MinecraftNetworkClient client)
    {
        byte[] uuid = GeneralUtils.GetUUId();
        MinecraftPlayer newPlayer = new MinecraftPlayer(username, uuid, client);
        newPlayer.NetworkClient.ClientDisconnect += () => { players.Remove(newPlayer); };
        players.Add(newPlayer);
        logger.Information("New player '{Username}' from {addresss} has joined the server ; uuid:{UuidHex}",username,client.IpAddress,uuid.ConvertBytesToHex());
        return newPlayer;
    }
            
}