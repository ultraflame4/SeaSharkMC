using System.IO;
using fNbt;

namespace SeaSharkMC.Networking.MinecraftPackets.Client;

// https://wiki.vg/index.php?title=Protocol&oldid=16681#Join_Game

public class JoinGamePacket:MinecraftBasePacket
{
    public int EntityID = 0;
    public bool IsHardcore = false;
    public World.Gamemode Gamemode = World.Gamemode.Spectator;
    public World.Gamemode? PrevGamemode = null;
    public string[] WorldNames;
    public NbtCompound DimensionCodec;
    
    public JoinGamePacket() : base(0x24)
    {
        
    }
    protected override void OnDataToBytes(MemoryStream dataStream)
    {
        throw new System.NotImplementedException();
    }
}