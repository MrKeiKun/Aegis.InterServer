using System;
using System.IO;
using System.Text;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_PING_LIVE, 2)]
    public class ZI_PING_LIVE : PacketBase
    {
        public ZI_PING_LIVE()
        {
            Command = (ushort)PACKET_COMMAND.ZI_PING_LIVE;
        }

        public ZI_PING_LIVE(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                }
            }
        }

        public override void WriteTo(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}