using SharpNBT;

namespace SeaSharkMC.Game;

public class ServerConfig
{
    public readonly int render_distance = 12;
    public readonly int max_players = 12;
    public readonly bool reducedDebugInfo = false;
    public readonly bool enableRespawnScreen = true;
    public readonly CompoundTag dimension_codec;
    public readonly CompoundTag dimension;
    
    public readonly World[] worlds;
    public readonly World default_world;

    public ServerConfig(CompoundTag dimensionCodec, World[] worlds, World defaultWorld)
    {
        dimension_codec = dimensionCodec;
        this.dimension =
                ((dimension_codec["minecraft:dimension_type"] as CompoundTag)["value"] as ListTag)[0] as CompoundTag;
        this.worlds = worlds;
        default_world = defaultWorld;
    }
}