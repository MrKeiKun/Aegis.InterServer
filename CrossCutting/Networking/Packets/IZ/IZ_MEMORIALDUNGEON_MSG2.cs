using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_MEMORIALDUNGEON_MSG2)]
    public class IZ_MEMORIALDUNGEON_MSG2 : PacketVarSize
    {
        public enum EnumNotify
        {
            MEMORIALDUNGEON_SUBSCRIPTION_ERROR_UNKNOWN = 0x0,
            MEMORIALDUNGEON_SUBSCRIPTION_ERROR_DUPLICATE = 0x1,
            MEMORIALDUNGEON_SUBSCRIPTION_ERROR_RIGHT = 0x2,
            MEMORIALDUNGEON_SUBSCRIPTION_ERROR_EXIST = 0x3,
            MEMORIALDUNGEON_SUBSCRIPTION_CANCEL_FAIL = 0x4,
            MEMORIALDUNGEON_SUBSCRIPTION_CANCEL_SUCCESS = 0x5,
            MEMORIALDUNGEON_CREATE_FAIL = 0x6,
            MEMORIALDUNGEON_SUBSCRIPTION_ERROR_CLOSE = 0x7,
        }

        public int AID { get; set; }
        public int GID { get; set; }
        public EnumNotify Notify { get; set; }
        public string DungeonName { get; set; }

        public IZ_MEMORIALDUNGEON_MSG2()
        {
            Command = (ushort)PACKET_COMMAND.IZ_MEMORIALDUNGEON_MSG2;
        }

        public IZ_MEMORIALDUNGEON_MSG2(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Notify = (EnumNotify)br.ReadInt32();
                    DungeonName = br.ReadCString(Length - 16);

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
            bw.Write(Length);
            bw.Write(AID);
            bw.Write(GID);
            bw.Write((int)Notify);
            bw.WriteCString(DungeonName);
        }
    }
}
