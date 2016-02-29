using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_ACK_MAKE_GROUP, 35)]
    public class IZ_ACK_MAKE_GROUP : PacketBase
    {
        public int AID { get; set; }
        public int GRID { get; set; }
        public string GroupName { get; set; }
        public byte Result { get; set; }

        public IZ_ACK_MAKE_GROUP()
        {
            Command = (ushort) PACKET_COMMAND.IZ_ACK_MAKE_GROUP;
        }

        public IZ_ACK_MAKE_GROUP(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GRID = br.ReadInt32();
                    GroupName = br.ReadCString(24);
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
            bw.Write(AID);
            bw.Write(GRID);
            bw.WriteCString(GroupName, 24);
            bw.Write(Result);
        }
    }
}