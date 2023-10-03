using System.IO;

namespace SeaSharkMC.old.Networking.MinecraftPackets.Client;

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

    protected override void OnDataToBytes(MemoryStream dataStream)
    {
        dataStream.Write(uuid, 0, uuid.Length);
        dataStream.WriteVarIntString(username);
    }
    
}