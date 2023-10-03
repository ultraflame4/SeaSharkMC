using System.IO;
using System.Linq;
using SeaSharkMC.Game;
using SeaSharkMC.Networking.Datatypes;
using SharpNBT;
using VarInt = SeaSharkMC.Networking.Datatypes.VarInt;

namespace SeaSharkMC.Networking.Outgoing;

public class JoinGamePacket : OutgoingPacket
{
    int entity_id = 0;
    bool hardcore = false;
    byte gamemode = 1;
    sbyte previous_gamemode = -1;
    VarIntString[] worldNames;
    CompoundTag dimension_codec;
    CompoundTag dimension;
    string world_name;
    long hashed_seed = 0;
    VarInt max_players = 0;
    VarInt view_distance;
    bool reduced_debug_info = false;
    bool enable_respawn_screen = true;
    bool is_debug = false;
    bool is_flat = false;


    public JoinGamePacket(ServerConfig serverConfig,int entityId,bool hardcore,byte gamemode=1,sbyte previousGamemode=-1) : base(0x24)
    {
        entity_id = entityId;
        this.hardcore = hardcore;
        this.gamemode = gamemode;
        previous_gamemode = previousGamemode;
        worldNames = serverConfig.worlds.Select(x=>new VarIntString(x.name)).ToArray();
        dimension_codec = serverConfig.dimension_codec;
        dimension = serverConfig.dimension;
        world_name = serverConfig.default_world.name;
        hashed_seed = serverConfig.default_world.hashed_seed;
        max_players = serverConfig.default_world.name.Length;
        view_distance = serverConfig.render_distance;
        reduced_debug_info = serverConfig.reducedDebugInfo;
        enable_respawn_screen = serverConfig.enableRespawnScreen;
        is_debug = serverConfig.default_world.is_debug;
        is_flat = serverConfig.default_world.is_debug;
    }

    public override void WriteData(MemoryStream stream)
    {
        stream.WriteInt(entity_id);
        stream.WriteBool(hardcore);
        stream.WriteByte(gamemode);
        stream.WriteSbyte(previous_gamemode);
        new VarInt(worldNames.Length).WriteTo(stream);
        foreach (var name in worldNames)
        {
            name.WriteTo(stream);
        }
        
        using var bufferedWriter = BufferedTagWriter.Create(CompressionType.None, FormatOptions.Java);
        bufferedWriter.WriteTag(dimension_codec);
        bufferedWriter.WriteTag(dimension);
        bufferedWriter.CopyTo(stream);
        new VarIntString(world_name).WriteTo(stream);
        stream.WriteLong(hashed_seed);
        new VarInt(max_players).WriteTo(stream);
        new VarInt(view_distance).WriteTo(stream);
        stream.WriteBool(reduced_debug_info);
        stream.WriteBool(enable_respawn_screen);
        stream.WriteBool(is_debug);
        stream.WriteBool(is_flat);
    }
}