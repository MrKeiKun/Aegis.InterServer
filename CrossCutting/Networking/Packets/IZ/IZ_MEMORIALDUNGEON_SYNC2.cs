using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_MEMORIALDUNGEON_SYNC2)]
    public class IZ_MEMORIALDUNGEON_SYNC2 : PacketVarSize
    {
        public enum EnumEVENT
        {
            EVENT_CREATE = 0x0,
            EVENT_DESTROY = 0x1,
        }

        public int PartyID { get; set; }
        public string GroupName { get; set; }
        public int ExistZSID { get; set; }
        public int MemorialDungeonID { get; set; }
        public int Factor { get; set; }
        public EnumEVENT Event { get; set; }
        public string DungeonName { get; set; }

        public IZ_MEMORIALDUNGEON_SYNC2()
        {
            Command = (ushort)PACKET_COMMAND.IZ_MEMORIALDUNGEON_SYNC2;
        }

        public IZ_MEMORIALDUNGEON_SYNC2(byte[] packet)
                : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    PartyID = br.ReadInt32();
                    GroupName = br.ReadCString(24);
                    ExistZSID = br.ReadInt32();
                    MemorialDungeonID = br.ReadInt32();
                    Factor = br.ReadInt32();
                    Event = (EnumEVENT)br.ReadInt32();
                    DungeonName = br.ReadCString(Length - 48);

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
            bw.Write(PartyID);
            bw.WriteCString(GroupName, 24);
            bw.Write(ExistZSID);
            bw.Write(MemorialDungeonID);
            bw.Write(Factor);
            bw.Write((int)Event);
            bw.WriteCString(DungeonName);
        }
    }
}
