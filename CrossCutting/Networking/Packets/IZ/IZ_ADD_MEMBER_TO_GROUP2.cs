using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_ADD_MEMBER_TO_GROUP2, 81)]
    public class IZ_ADD_MEMBER_TO_GROUP2 : PacketBase
    {
        public int ReceiverAID { get; set; }
        public int AID { get; set; }
        public int Role { get; set; }
        public byte State { get; set; }
        public string GroupName { get; set; }
        public string CharacterName { get; set; }
        public string MapName { get; set; }
        public byte ItemPickupRule { get; set; }
        public byte ItemDivisionRule { get; set; }

        public IZ_ADD_MEMBER_TO_GROUP2()
        {
            Command = (ushort) PACKET_COMMAND.IZ_ADD_MEMBER_TO_GROUP2;
        }

        public IZ_ADD_MEMBER_TO_GROUP2(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    ReceiverAID = br.ReadInt32();
                    AID = br.ReadInt32();
                    Role = br.ReadInt32();
                    State = br.ReadByte();
                    GroupName = br.ReadCString(24);
                    CharacterName = br.ReadCString(24);
                    MapName = br.ReadCString(16);
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
            bw.Write(ReceiverAID);
            bw.Write(AID);
            bw.Write(Role);
            bw.Write(State);
            bw.WriteCString(GroupName, 24);
            bw.WriteCString(CharacterName, 24);
            bw.WriteCString(MapName, 16);
            bw.Write(ItemPickupRule);
            bw.Write(ItemDivisionRule);
        }
    }
}