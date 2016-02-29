using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_CHANGE_MEMBERPOS)]
    public class ZI_REQ_CHANGE_MEMBERPOS : PacketVarSize
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }
        public byte[] Data { get; set; }

        public ZI_REQ_CHANGE_MEMBERPOS()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_CHANGE_MEMBERPOS;
        }

        public ZI_REQ_CHANGE_MEMBERPOS(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Data = br.ReadBytes(Length - 12);

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
            bw.Write(GDID);
            bw.Write(AID);
            bw.Write(GID);
            bw.Write(Data);
        }
    }
}
