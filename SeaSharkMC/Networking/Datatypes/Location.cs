using System.IO;

namespace SeaSharkMC.Networking.Datatypes;

public struct Location
{
    int x, y, z;
    public Location(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public readonly void WriteTo(Stream stream)
    {
        long encoded = x & 0x3FFFFFF;
        encoded <<= 38;
        encoded |= z & 0x3FFFFFF;
        encoded <<= 12;
        encoded |= y & 0xFFF;
        stream.WriteLong(encoded);
    }
    
    public static Location ReadFrom(Stream stream)
    {
        long encoded = stream.ReadLong();
        int x = (int)(encoded >> 38);
        int y = (int)(encoded << 52 >> 52);
        int z = (int)(encoded << 26 >> 38);
        return new Location(x, y, z);
    }
}