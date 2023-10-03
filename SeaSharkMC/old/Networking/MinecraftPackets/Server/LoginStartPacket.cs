using System.IO;

namespace SeaSharkMC.old.Networking.MinecraftPackets.Server;

public class LoginStartPacket : MinecraftBasePacket
{
    private string playerUsername;

    public string PlayerUsername => playerUsername;

    public LoginStartPacket(string playerUsername) : base(0x00)
    {
        this.playerUsername = playerUsername;
    }

    public LoginStartPacket(GenericMinecraftPacket packet) : base(packet)
    {
        playerUsername = packet.Stream.ReadVarIntString();
    }

    protected override void OnDataToBytes(MemoryStream dataStream)
    {
        throw new System.NotImplementedException();
    }
}