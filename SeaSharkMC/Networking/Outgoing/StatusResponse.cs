using System.IO;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

public class StatusResponse : OutgoingPacket
{
    public string mc_version;
    public string protocol_version;
    public int max_players;
    public int current_players;
    public string description;


    public StatusResponse(string mcVersion, string protocolVersion, string description) : base(0x00)
    {
        mc_version = mcVersion;
        protocol_version = protocolVersion;
        this.description = description;
    }

    public override void WriteData(MemoryStream stream)
    {
        var json = $@" {{
     ""version"": {{
         ""name"": ""{mc_version}"",
         ""protocol"": ""{protocol_version}""
     }},
     ""players"": {{
         ""max"": {max_players},
         ""online"": {current_players},
         ""sample"": []
     }},
     ""description"": {{
         ""text"": ""{description}""
     }},
     ""enforcesSecureChat"": false,
     ""previewsChat"": false
 }}";

        new VarIntString(json).WriteTo(stream);
    }
}