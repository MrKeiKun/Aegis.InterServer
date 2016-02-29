using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_MAKE_GROUP2, 36)]
    public class ZI_MAKE_GROUP2 : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public string GroupName { get; set; }
        public byte ItemPickupRule { get; set; }
        public byte ItemDivisionRule { get; set; }

        public ZI_MAKE_GROUP2()
        {
            Command = (ushort) PACKET_COMMAND.ZI_MAKE_GROUP2;
        }

        public ZI_MAKE_GROUP2(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    GroupName = br.ReadCString(24);
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
            bw.Write(AID);
            bw.Write(GID);
            bw.WriteCString(GroupName, 24);
            bw.Write(ItemPickupRule);
            bw.Write(ItemDivisionRule);
        }
    }
}