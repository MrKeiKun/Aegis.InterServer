using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_JOIN_CLAN, 14)]
    public class ZI_REQ_JOIN_CLAN : PacketBase
    {
        public int ClanID { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }

        public ZI_REQ_JOIN_CLAN()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_JOIN_CLAN;
        }

        public ZI_REQ_JOIN_CLAN(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    ClanID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();

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
            bw.Write(ClanID);
            bw.Write(AID);
            bw.Write(GID);
        }
    }
}