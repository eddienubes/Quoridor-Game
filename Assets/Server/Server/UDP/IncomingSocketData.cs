using System;

namespace QuorridorAI.UDP;

public class IncomingSocketData
{
    public byte[] Data { get; } = new byte[1024];

    public int BufSize => Data.Length;
}