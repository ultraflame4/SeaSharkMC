using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SeaSharkMC.Networking.Datatypes;
using Serilog;

namespace SeaSharkMC.Networking.MinecraftPackets;

/// <summary>
/// A generic minecraft packet that only knows the packet length and packet id. While it also stores the data bytes, it does not know what it contains
/// </summary>
public class GenericMinecraftPacket
{
    protected int packetLength;
    protected int packetId;
    protected MemoryStream bytesStream;
    protected int totalSize;
    protected MinecraftNetworkClient? sourceClient;


    public int PacketLength => packetLength;
    public int PacketId => packetId;

    public MemoryStream Stream => bytesStream;

    /// <summary>
    /// The total size of the entire packet in number of bytes. This include the size of the packet length, packet id and data
    /// </summary>
    public int TotalSize => totalSize;

    /// <summary>
    /// The client it originated from. If null, source is server
    /// </summary>
    public MinecraftNetworkClient? SourceClient => sourceClient;
    
    private GenericMinecraftPacket(byte[] bytesArray, int offset=0, MinecraftNetworkClient? sourceClient = null)
    {
        bytesStream = new MemoryStream(bytesArray,offset,bytesArray.Length-offset); // temp solution, todo fix ltr, very inefficient, converting entire array into memory stream repeatedly
        packetLength = VarInt.ReadFrom(bytesStream);
        int packetLengthByteSize = (int)bytesStream.Position;
        packetId = VarInt.ReadFrom(bytesStream);

        this.sourceClient = sourceClient;

        totalSize = packetLength+packetLengthByteSize;
    }
    
    /// <summary>
    /// Creates 1 or more packet frames from the bytes array
    /// </summary>
    /// <param name="bytesArray"></param>
    /// <param name="sourceClient">Null if server</param>
    public static GenericMinecraftPacket[] Create(byte[] bytesArray, MinecraftNetworkClient? sourceClient = null)
    {
        int offset = 0;
        List<GenericMinecraftPacket> frames = new List<GenericMinecraftPacket>();
        
        while (true)
        {
            GenericMinecraftPacket frame = new GenericMinecraftPacket(bytesArray, offset, sourceClient);
            
            if (frame.TotalSize < 2)
            {
                break;
            }
            
            frames.Add(frame);
            
            if (offset>=bytesArray.Length-1)
            {
                break;
            }

            offset += frame.TotalSize;
        }

        return frames.ToArray();
    }
}