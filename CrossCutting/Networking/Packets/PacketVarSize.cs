using System.IO;

namespace Aegis.CrossCutting.Network.Packets
{
    public abstract class PacketVarSize : PacketBase
    {
        public ushort Length { get; set; }

        public PacketVarSize()
        {
        }

        public PacketVarSize(byte[] packet)
            : base(packet)
        {
        }

        public override ushort GetHeaderSize()
        {
            return (ushort) (base.GetHeaderSize() + sizeof (ushort));
        }

        public void SetLength(BinaryWriter bw)
        {
            bw.Seek(2, SeekOrigin.Begin);
            bw.Write((ushort) bw.BaseStream.Length);
        }
    }
}