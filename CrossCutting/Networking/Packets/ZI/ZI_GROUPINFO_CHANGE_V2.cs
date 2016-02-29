using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_GROUPINFO_CHANGE_V2, 20)]
    public class ZI_GROUPINFO_CHANGE_V2 : PacketBase
    {
        public int GRID { get; set; }
        public int MasterAID { get; set; }
        public int FamilyGroup { get; set; }
        public int ExpOption { get; set; }
        public byte ItemPickupRule { get; set; }
        public byte ItemDivisionRule { get; set; }

        public ZI_GROUPINFO_CHANGE_V2()
        {
            Command = (ushort) PACKET_COMMAND.ZI_GROUPINFO_CHANGE_V2;
        }

        public ZI_GROUPINFO_CHANGE_V2(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GRID = br.ReadInt32();
                    MasterAID = br.ReadInt32();
                    FamilyGroup = br.ReadInt32();
                    ExpOption = br.ReadInt32();
                    ItemPickupRule = br.ReadByte();
                    ItemDivisionRule = br.ReadByte();

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
            bw.Write(GRID);
            bw.Write(MasterAID);
            bw.Write(FamilyGroup);
            bw.Write(ExpOption);
            bw.Write(ItemPickupRule);
            bw.Write(ItemDivisionRule);
        }
    }
}
