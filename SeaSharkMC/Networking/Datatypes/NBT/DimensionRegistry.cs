using fNbt;

namespace SeaSharkMC.Networking.Datatypes.NBT;

public class DimensionRegistry : WNbtList<DimensionRegistryItem,NbtCompound>
{

    public DimensionRegistry() : base("values")
    {
        AddDimension("minecraft:overworld",
            new DimensionRegistryItemElement(
                false,
                true,
                0f,
                "minecraft:infiniburn_overworld",
                false,
                true,
                true,
                "minecraft:overworld",
                true,
                256,
                1f,
                false,
                false
                ));
        
        AddDimension("minecraft:overworld_caves",
            new DimensionRegistryItemElement(
                false,
                true,
                0f,
                "minecraft:infiniburn_overworld",
                false,
                true,
                true,
                "minecraft:overworld",
                true,
                256,
                1f,
                false,
                true
            ));
        
        AddDimension("minecraft:the_nether",
            new DimensionRegistryItemElement(
                true,
                false,
                0.1f,
                "minecraft:infiniburn_nether",
                true,
                false,
                false,
                "minecraft:the_nether",
                false,
                128,
                8f,
                true,
                true
            ));
        
        AddDimension("minecraft:the_end",
            new DimensionRegistryItemElement(
                false,
                false,
                0f,
                "minecraft:infiniburn_end",
                false,
                false,
                false,
                "minecraft:the_end",
                true,
                256,
                1f,
                true,
                true
            ));
    }
    
    public void AddDimension(string name, DimensionRegistryItemElement element)
    {
        Add(new DimensionRegistryItem(name,element,Count));
    }
}