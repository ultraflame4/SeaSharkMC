using System.IO;
using System.Text;
using SeaSharkMC.Networking.Datatypes;

namespace SeaSharkMC.Networking.Outgoing;

/// <summary>
/// Clientbound plugin message packet
/// </summary>
public class PluginMessagePacket_C: OutgoingPacket
{
    public readonly string channel;
    public readonly byte[] data;
    
    public PluginMessagePacket_C(string channel, byte[] data) : base(0x17)
    {
        this.channel = channel;
        this.data = data;
    }
    public PluginMessagePacket_C(string channel, string data) : base(0x17)
    {
        this.channel = channel;
        this.data = Encoding.UTF8.GetBytes(data);
    }

    public override void WriteData(MemoryStream stream)
    {
        new VarIntString(channel).WriteTo(stream);
        stream.Write(data);
    }
}