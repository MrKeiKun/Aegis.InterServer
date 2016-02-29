using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_MEMORIALDUNGEON_SUBSCRIPTION2)]
    public class ZI_MEMORIALDUNGEON_SUBSCRIPTION2 : PacketVarSize
    {
        public string NickName { get; set; }
        public int GRID { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }
        public string DungeonName { get; set; }

        public ZI_MEMORIALDUNGEON_SUBSCRIPTION2()
        {
            Command = (ushort) PACKET_COMMAND.ZI_MEMORIALDUNGEON_SUBSCRIPTION2;
        }

        public ZI_MEMORIALDUNGEON_SUBSCRIPTION2(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    NickName = br.ReadCString(61);
                    GRID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    DungeonName = br.ReadCString(Length - 77);

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
            bw.WriteCString(NickName, 61);
            bw.Write(GRID);
            bw.Write(AID);
            bw.Write(GID);
            bw.WriteCString(DungeonName);
        }
    }
}