using System;
using System.IO;
using System.Numerics;
using SeaSharkMC.Networking.Datatypes;
using Vector3 = Abacus.DoublePrecision.Vector3;


namespace SeaSharkMC.Networking.Outgoing;

[Flags]
public enum PlayerPositionAndLookFlags : byte
{
    RELATIVE_X = 0x01,
    RELATIVE_Y = 0x02,
    RELATIVE_Z = 0x03,
    RELATIVE_PITCH = 0x0,
    RELATIVE_YAW = 0x10,
}

public class PlayerPositionAndLookPacket_C:OutgoingPacket
{

    public readonly Vector3 position;
    public readonly float yaw;
    public readonly float pitch;
    public readonly int teleportConfirmId;
    public readonly PlayerPositionAndLookFlags flags;
    
    
    public PlayerPositionAndLookPacket_C(Vector3 position, float yaw, float pitch, int teleportConfirmId, PlayerPositionAndLookFlags flags=0) : base(0x34)
    {
        this.position = position;
        this.yaw = yaw;
        this.pitch = pitch;
        this.teleportConfirmId = teleportConfirmId;
        this.flags = flags;
    }

    public override void WriteData(MemoryStream stream)
    {
        stream.WriteDouble(position.X);
        stream.WriteDouble(position.Y);
        stream.WriteDouble(position.Z);
        stream.WriteFloat(yaw);
        stream.WriteFloat(pitch);
        stream.WriteByte((byte)flags);
        new VarInt(teleportConfirmId).WriteTo(stream);
        
    }
}