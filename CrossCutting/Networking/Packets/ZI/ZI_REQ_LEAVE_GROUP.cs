using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_LEAVE_GROUP, 10)]
    public class ZI_REQ_LEAVE_GROUP : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }

        public ZI_REQ_LEAVE_GROUP()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_LEAVE_GROUP;
        }

        public ZI_REQ_LEAVE_GROUP(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
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
            bw.Write(AID);
            bw.Write(GID);
        }
    }
}