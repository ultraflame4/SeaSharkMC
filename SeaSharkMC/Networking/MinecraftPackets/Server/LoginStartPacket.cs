namespace SeaSharkMC.Networking.MinecraftPackets;

public class LoginStartPacket: MinecraftBasePacket
{
    private string playerUsername;

    public string PlayerUsername => playerUsername;

    public LoginStartPacket(MinecraftPacketFrame packetFrame) : base(packetFrame)
    {
        (playerUsername, int size) = PacketDataUtils.ReadVarIntString(bytesArray, packetFrame.PacketDataOffset);
    }

    protected override byte[] GetDataByteArray()
    {
        throw new System.NotImplementedException();
    }
}