namespace SeaSharkMC.Networking.MinecraftPackets.Client;

public class EncryptionRequestPacket : MinecraftBasePacket
{

    public EncryptionRequestPacket(int packetId) : base(0x01)
    {
        
    }
    protected override byte[] GetDataByteArray()
    {
        throw new System.NotImplementedException();
    }
}