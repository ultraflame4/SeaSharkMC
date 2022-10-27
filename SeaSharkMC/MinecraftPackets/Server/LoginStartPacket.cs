namespace SeaSharkMC.MinecraftPackets;

public class LoginStartPacket: MinecraftServerPacket
{
    private string playerUsername;

    public string PlayerUsername => playerUsername;

    public LoginStartPacket(byte[] bytesArray) : base(bytesArray)
    {
        (playerUsername, int size) = PacketDataUtils.ReadVarIntString(bytesArray, packetDataOffset);
    }
}