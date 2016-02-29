using System;
using System.IO;
using System.Text;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_VERSION, 6)]
    public class ZI_VERSION : PacketBase
    {
        public int Version { get; set; }

        public ZI_VERSION()
        {
            Command = (ushort)PACKET_COMMAND.ZI_VERSION;
        }

        public ZI_VERSION(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Version = br.ReadInt32();
                }
            }
        }

        public override void WriteTo(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}