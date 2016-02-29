using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_LEAVE_GROUP, 10)]
    public class IZ_LEAVE_GROUP : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }

        public IZ_LEAVE_GROUP()
        {
            Command = (ushort) PACKET_COMMAND.IZ_LEAVE_GROUP;
        }

        public IZ_LEAVE_GROUP(byte[] packet)
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