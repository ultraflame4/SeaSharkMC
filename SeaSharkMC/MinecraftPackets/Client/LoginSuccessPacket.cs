namespace SeaSharkMC.MinecraftPackets.Client;

public class LoginSuccessPacket : MinecraftBasePacket
{
    public LoginSuccessPacket(int packetLength, int packetId, int packetDataOffset, int packetDataLength) : base(packetLength, packetId, packetDataOffset, packetDataLength)
    {
        
    }

    // protected override byte[] GetDataByteArray()
    // {
    //     
    // }
}