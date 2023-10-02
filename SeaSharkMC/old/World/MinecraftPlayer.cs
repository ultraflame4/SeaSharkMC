﻿using SeaSharkMC.Networking;

namespace SeaSharkMC.World;

public class MinecraftPlayer
{
    public readonly byte[] uuid;
    public readonly string Username;
    public readonly MinecraftNetworkClient NetworkClient;

    public MinecraftPlayer(string username,byte[] uuid, MinecraftNetworkClient networkClient)
    {
        this.uuid = uuid;
        Username = username;
        NetworkClient = networkClient;
    }
}