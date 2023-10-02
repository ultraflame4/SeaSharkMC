using fNbt;

namespace SeaSharkMC.Networking.Datatypes.NBT;

public class DimensionCodec : WNbtType<NbtCompound>
{
     public NbtString type = new NbtString("type","minecraft:dimension_type");
     public DimensionRegistry values = new DimensionRegistry();

     public DimensionCodec() : base(new NbtCompound("minecraft:dimension_type"))
     {
          
     }
}