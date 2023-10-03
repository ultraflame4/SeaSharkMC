namespace SeaSharkMC.old.Networking.Datatypes.NBT;

public class DimensionRegistryItemElement
{
    bool piglin_safe;
    bool natural;
    float ambient_light;
    string infiniburn;
    bool respawn_anchor_works;
    bool has_skylight;
    bool bed_works;
    string effects;
    bool has_raids;
    int logical_height;
    float coordinate_scale;
    bool ultrawarm;
    bool has_ceiling;

    public DimensionRegistryItemElement(
        bool piglinSafe,
        bool natural,
        float ambientLight,
        string infiniburn,
        bool respawnAnchorWorks,
        bool hasSkylight,
        bool bedWorks,
        string effects,
        bool hasRaids,
        int logicalHeight,
        float coordinateScale,
        bool ultrawarm,
        bool hasCeiling)
    {
        piglin_safe = piglinSafe;
        this.natural = natural;
        ambient_light = ambientLight;
        this.infiniburn = infiniburn;
        respawn_anchor_works = respawnAnchorWorks;
        has_skylight = hasSkylight;
        bed_works = bedWorks;
        this.effects = effects;
        has_raids = hasRaids;
        logical_height = logicalHeight;
        coordinate_scale = coordinateScale;
        this.ultrawarm = ultrawarm;
        has_ceiling = hasCeiling;
    }
}