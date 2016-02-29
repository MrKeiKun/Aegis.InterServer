using System;
using System.IO;
using System.Text;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_INSTANTMAP_ALLOW, 7)]
    public class ZI_INSTANTMAP_ALLOW : PacketBase
    {
        public enum EnumWhy : int
        {
            WHY_ZSVR_SETTING = 0x0,
            WHY_AGITWAR_START = 0x1,
            WHY_AGITWAR_END = 0x2,
        }

        public EnumWhy Why { get; set; }
        public bool Allow { get; set; }

        public ZI_INSTANTMAP_ALLOW()
        {
            Command = (ushort)PACKET_COMMAND.ZI_INSTANTMAP_ALLOW;
        }

        public ZI_INSTANTMAP_ALLOW(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Why = (EnumWhy)br.ReadInt32();
                    Allow = br.ReadBoolean();
                }
            }
        }

        public override void WriteTo(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}
