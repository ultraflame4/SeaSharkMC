using System;
using Serilog;

namespace SeaSharkMC.MinecraftPackets.Client;

public class LoginSuccessPacket : MinecraftBasePacket
{
    private byte[] uuid;
    private string username;
    
    public LoginSuccessPacket(byte[] uuid, string username) : base(0x02)
    {
        this.uuid = uuid;
        this.username = username;
        this.ToBytesArray();
    }

    protected override byte[] GetDataByteArray()
    {
        int usernameSize = PacketDataUtils.EvaluateVarIntString(username);
        byte[] data = new byte[uuid.Length + usernameSize];
        Array.Copy(uuid,0,data,0,uuid.Length);
        PacketDataUtils.WriteVarIntString(data, username, uuid.Length);

        return data;
    }
}