using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_MEMORIALDUNGEON_VISA, 10)]
    public class ZI_MEMORIALDUNGEON_VISA : PacketBase
    {
        public int GID { get; set; }
        public int AID { get; set; }

        public ZI_MEMORIALDUNGEON_VISA()
        {
            Command = (ushort) PACKET_COMMAND.ZI_MEMORIALDUNGEON_VISA;
        }

        public ZI_MEMORIALDUNGEON_VISA(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GID = br.ReadInt32();
                    AID = br.ReadInt32();

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
            bw.Write(AID);
        }
    }
}
