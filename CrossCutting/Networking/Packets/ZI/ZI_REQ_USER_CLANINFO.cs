using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_USER_CLANINFO, 6)]
    public class ZI_REQ_USER_CLANINFO : PacketBase
    {
        public int GID { get; set; }

        public ZI_REQ_USER_CLANINFO()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_USER_CLANINFO;
        }

        public ZI_REQ_USER_CLANINFO(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
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
            bw.Write(GID);
        }
    }
}