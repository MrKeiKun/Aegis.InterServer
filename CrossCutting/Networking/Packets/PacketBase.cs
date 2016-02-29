using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets
{
    public abstract class PacketBase
    {
        public ushort Command { get;  set; }
        public byte[] Data { get; protected set; }

        public PacketBase()
        {
        }

        public PacketBase(byte[] packet)
        {
            Command = (ushort) ((packet[1] << 8) | packet[0]);
        }

        public virtual ushort GetHeaderSize()
        {
            return sizeof (ushort);
        }

        public abstract void WriteTo(BinaryWriter bw);
    }
}