using fNbt;

namespace SeaSharkMC.old.Networking.Datatypes.NBT;

public class DimensionRegistryItem : WNbtType<NbtCompound>
{
    string name;
    int id;
    private DimensionRegistryItemElement elements;
    
    public DimensionRegistryItem(string name, DimensionRegistryItemElement elements, int id) : base(new NbtCompound())
    {
        this.name = name;
        this.elements = elements;
        this.id = id;
    }
}