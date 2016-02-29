using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_MEMORIALDUNGEON_NOTIFY, 18)]
    public class IZ_MEMORIALDUNGEON_NOTIFY : PacketBase
    {
        public enum EnumTYPE
        {
            TYPE_NOTIFY = 0x0,
            TYPE_DESTROY_LIVE_TIMEOUT = 0x1,
            TYPE_DESTROY_ENTER_TIMEOUT = 0x2,
            TYPE_DESTROY_USER_REQUEST = 0x3,
            TYPE_CREATE_FAIL = 0x4,
        }

        public int AID { get; set; }
        public int GID { get; set; }
        public EnumTYPE Type { get; set; }
        public int EnterLimitDate { get; set; }

        public IZ_MEMORIALDUNGEON_NOTIFY()
        {
            Command = (ushort)PACKET_COMMAND.IZ_MEMORIALDUNGEON_NOTIFY;
        }

        public IZ_MEMORIALDUNGEON_NOTIFY(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Type = (EnumTYPE)br.ReadInt32();
                    EnterLimitDate = br.ReadInt32();

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
            bw.Write((int)Type);
            bw.Write(EnterLimitDate);
        }
    }
}
