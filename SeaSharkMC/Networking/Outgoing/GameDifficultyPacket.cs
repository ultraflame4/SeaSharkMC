using System.IO;
using SeaSharkMC.Game;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public class GameDifficultyPacket : OutgoingPacket
{
    private readonly GameDifficulty difficulty;
    private readonly bool locked;

    public GameDifficultyPacket(GameDifficulty difficulty, bool locked) : base(0x0D)
    {
        this.difficulty = difficulty;
        this.locked = locked;
    }

    public override void WriteData(MemoryStream stream)
    {
        stream.WriteByte((byte)difficulty);
        stream.WriteBool(locked);
    }
}