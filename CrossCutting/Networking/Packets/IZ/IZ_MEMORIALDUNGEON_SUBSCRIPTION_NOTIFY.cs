using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_MEMORIALDUNGEON_SUBSCRIPTION_NOTIFY, 12)]
    public class IZ_MEMORIALDUNGEON_SUBSCRIPTION_NOTIFY : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public short PriorityOrderNum { get; set; }

        public IZ_MEMORIALDUNGEON_SUBSCRIPTION_NOTIFY()
        {
            Command = (ushort) PACKET_COMMAND.IZ_MEMORIALDUNGEON_SUBSCRIPTION_NOTIFY;
        }

        public IZ_MEMORIALDUNGEON_SUBSCRIPTION_NOTIFY(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    PriorityOrderNum = br.ReadInt16();

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
            bw.Write(PriorityOrderNum);
        }
    }
}
