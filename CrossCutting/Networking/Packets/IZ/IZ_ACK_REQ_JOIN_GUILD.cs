using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_ACK_REQ_JOIN_GUILD, 15)]
    public class IZ_ACK_REQ_JOIN_GUILD : PacketBase
    {
        public int AID { get; set; }
        public int ReqAID { get; set; }
        public int GDID { get; set; }
        public byte Answer { get; set; }

        public IZ_ACK_REQ_JOIN_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_ACK_REQ_JOIN_GUILD;
        }

        public IZ_ACK_REQ_JOIN_GUILD(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    ReqAID = br.ReadInt32();
                    GDID = br.ReadInt32();
                    Answer = br.ReadByte();

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
            bw.Write(ReqAID);
            bw.Write(GDID);
            bw.Write(Answer);
        }
    }
}
