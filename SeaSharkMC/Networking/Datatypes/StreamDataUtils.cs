﻿using System;
using System.IO;

namespace SeaSharkMC.Networking.Datatypes;

public static class StreamDataUtils
{
    
    public static byte[] ReadBytes(this Stream stream, int count)
    {
        byte[] buffer = new byte[count];
        stream.Read(buffer, 0, count);
        return buffer;
    }

    
    public static void WriteBool(this Stream stream, bool value)
    {
        if (value)
        {
            stream.WriteByte(0x01);
        }
        else
        {
            stream.WriteByte(0x00);
        }
    }

    public static bool ReadBool(this Stream stream)
    {
        return (byte)stream.ReadByte() == 0x01;
    }

    public static short ReadShort(this Stream stream)
    {
        return (short)((stream.ReadByte() << 8) | stream.ReadByte());
    }
    
    public static ushort ReadUShort(this Stream stream)
    {
        return (ushort)((stream.ReadByte() << 8) | stream.ReadByte());
    }
    
    public static int ReadInt(this Stream stream)
    {
        return BitConverter.ToInt32(stream.ReadBytes(4), 0);
    }

    public static long ReadLong(this Stream stream)
    {
        return BitConverter.ToInt64(stream.ReadBytes(8), 0);
    }
    
    public static float ReadFloat(this Stream stream)
    {
        return BitConverter.ToSingle(stream.ReadBytes(4), 0);
    }
    public static double ReadDouble(this Stream stream)
    {
        return BitConverter.ToDouble(stream.ReadBytes(8), 0);
    }
}