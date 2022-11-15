using System;
using System.Text;
using SeaSharkMC.Networking.MinecraftPackets;
using Serilog;

namespace SeaSharkMC;

public static class GeneralUtils
{
    public static byte[] GetUUId()
    {
        Guid uuid = Guid.NewGuid();
        byte[] uuidBytes = uuid.ToByteArray();
        return uuidBytes;
    }

}