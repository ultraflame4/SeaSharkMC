using System.IO;
using System.Text;

namespace SeaSharkMC.Networking.Datatypes;

public class VarIntString
{
    private string value;
    public VarIntString(string value) { this.value = value; }

    public static implicit operator VarIntString(string value) { return new VarIntString(value); }
    public static implicit operator string(VarIntString value) { return value.value; }


    /// <summary>
    /// Reads a string prefixed with a var int from a string
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static VarIntString ReadFrom(Stream stream) { return ReadFrom(stream, out _); }

    /// <summary>
    /// Reads a string prefixed with a var int from a string
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="size">Byte size of the VarIntString (including the VarInt prefix)</param>
    /// <returns></returns>
    public static VarIntString ReadFrom(Stream stream, out int size)
    {
        int stringLength = VarInt.ReadFrom(stream, out int varIntSize);
        size = stringLength + varIntSize;
        byte[] stringBytes = new byte[stringLength];
        stream.ReadExactly(stringBytes, 0, stringLength);
        return new VarIntString(Encoding.UTF8.GetString(stringBytes));
    }
}