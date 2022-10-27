using System;
using System.Collections;
using System.Collections.Generic;
using Serilog;

namespace SeaSharkMC.MinecraftPackets;

public class MinecraftPacketFrame
{
    protected int packetLength;
    protected int packetId;
    protected int packetDataOffset;
    protected int packetDataLength;
    protected byte[] bytesArray;
    protected int totalSize;
    protected NetworkClient? sourceClient;

    public int PacketLength => packetLength;

    public int PacketId => packetId;

    public int PacketDataOffset => packetDataOffset;

    public int PacketDataLength => packetDataLength;

    public byte[] BytesArray => bytesArray;

    /// <summary>
    /// The total size of the entire packet in number of bytes. This include the size of the packet length, packet id and data
    /// </summary>
    public int TotalSize => totalSize;

    /// <summary>
    /// The client it originated from. If null, source is server
    /// </summary>
    public NetworkClient? SourceClient => sourceClient;



    private MinecraftPacketFrame(byte[] bytesArray, int offset=0, NetworkClient? sourceClient = null)
    {
        (packetLength, int aSize) = PacketDataUtils.ReadVarInt(bytesArray,offset);
        (packetId, int bSize) = PacketDataUtils.ReadVarInt(bytesArray, aSize+offset);

        packetDataOffset = aSize + bSize;
        packetDataLength = packetLength - bSize;
        totalSize = aSize + packetLength;
        this.sourceClient = sourceClient;
        this.bytesArray = new byte[totalSize];
        Array.Copy(bytesArray,offset,this.bytesArray,0,totalSize);
    }
    
    /// <summary>
    /// Creates 1 or more packet frames from the bytes array
    /// </summary>
    /// <param name="bytesArray"></param>
    /// <param name="sourceClient">Null if server</param>
    public static MinecraftPacketFrame[] Create(byte[] bytesArray, NetworkClient? sourceClient = null)
    {
        int offset = 0;
        List<MinecraftPacketFrame> frames = new List<MinecraftPacketFrame>();
        
        while (true)
        {
            MinecraftPacketFrame frame = new MinecraftPacketFrame(bytesArray, offset, sourceClient);
            
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