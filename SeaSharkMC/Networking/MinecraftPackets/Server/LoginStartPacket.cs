using System.IO;

namespace SeaSharkMC.Networking.MinecraftPackets;

public class LoginStartPacket : MinecraftBasePacket
{
    private string playerUsername;

    public string PlayerUsername => playerUsername;

    public LoginStartPacket(RawMinecraftPacket packet) : base(packet)
    {
        playerUsername = packet.Stream.ReadVarIntString();
    }

    protected override void OnDataToBytes(MemoryStream dataStream)
    {
        throw new System.NotImplementedException();
    }
}