using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_DELETE_MEMBER_FROM_GROUP, 35)]
    public class IZ_DELETE_MEMBER_FROM_GROUP : PacketBase
    {
        public int ReceiverAID { get; set; }
        public int AID { get; set; }
        public string CharacterName { get; set; }
        public byte Result { get; set; }

        public IZ_DELETE_MEMBER_FROM_GROUP()
        {
            Command = (ushort) PACKET_COMMAND.IZ_DELETE_MEMBER_FROM_GROUP;
        }

        public IZ_DELETE_MEMBER_FROM_GROUP(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    ReceiverAID = br.ReadInt32();
                    AID = br.ReadInt32();
                    CharacterName = br.ReadCString(24);
                    Result = br.ReadByte();

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
            bw.WriteCString(CharacterName, 24);
            bw.Write(Result);
        }
    }
}