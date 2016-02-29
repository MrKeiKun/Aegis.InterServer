using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_ACK_CHK_PROCESSTIME, 10)]
    public class ZI_ACK_CHK_PROCESSTIME : PacketBase
    {
        public int InterProcessTime { get; set; }
        public int ZoneProcessTime { get; set; }

        public ZI_ACK_CHK_PROCESSTIME()
        {
            Command = (ushort) PACKET_COMMAND.ZI_ACK_CHK_PROCESSTIME;
        }

        public ZI_ACK_CHK_PROCESSTIME(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    InterProcessTime = br.ReadInt32();
                    ZoneProcessTime = br.ReadInt32();

                    if (ms.Position != ms.Length)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        public override void WriteTo(BinaryWriter bw)
        {
            bw.Write(Command);
            bw.Write(InterProcessTime);
            bw.Write(ZoneProcessTime);
        }
    }
}
