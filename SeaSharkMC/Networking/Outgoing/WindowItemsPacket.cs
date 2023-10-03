using System.IO;
using SeaSharkMC.Game.Inventory;
using SeaSharkMC.Networking.Datatypes;
using SharpNBT;
using VarInt = SeaSharkMC.Networking.Datatypes.VarInt;

namespace SeaSharkMC.Networking.Outgoing;

public class WindowItemsPacket : OutgoingPacket
{
    /// <summary>
    /// Window ID for the player's inventory.
    /// </summary>
    public const int PLAYER_INVENTORY_ID = 0;

    public byte windowId;
    public Slot[] slots;

    public WindowItemsPacket(int windowId, Slot[] slots) : base(0x13) { this.slots = slots; }

    public override void WriteData(MemoryStream stream)
    {
        stream.WriteByte(windowId);
        stream.WriteShort((short) slots.Length);
        foreach (var slot in slots)
        {
            slot.WriteBytes(stream);
        }
    }
}