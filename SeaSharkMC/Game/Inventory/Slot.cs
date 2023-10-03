using System.IO;
using SeaSharkMC.Networking.Datatypes;
using SharpNBT;
using VarInt = SeaSharkMC.Networking.Datatypes.VarInt;

namespace SeaSharkMC.Game.Inventory;

public class Slot
{
    public bool present;
    public int itemId;
    public byte count = 0;
    public CompoundTag? item_nbt;

    public Slot(bool present = false, int itemId = 0, byte count =0, CompoundTag? itemNbt = null)
    {
        this.present = present;
        this.itemId = itemId;
        this.count = count;
        item_nbt = itemNbt;
    }

    public void WriteBytes(MemoryStream stream)
    {
        stream.WriteBool(present);
        if (!present) return;
        new VarInt(itemId).WriteTo(stream);
        new VarInt(count).WriteTo(stream);
        stream.WriteNBT(item_nbt);
    }
}