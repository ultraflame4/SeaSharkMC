using System.IO;
using SeaSharkMC.Game;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public class PlayerAbilitiesPacket_C: OutgoingPacket
{
    public readonly PlayerAbilities abilities;

    public PlayerAbilitiesPacket_C(PlayerAbilities abilities) : base(0x30) { this.abilities = abilities; }

    public override void WriteData(MemoryStream stream)
    {
        stream.WriteByte((byte) abilities.flags);
        stream.WriteFloat(abilities.flyingSpeed);
        stream.WriteFloat(abilities.fovModifer);
    }
}