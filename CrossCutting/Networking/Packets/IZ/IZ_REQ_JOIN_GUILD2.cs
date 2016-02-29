using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_REQ_JOIN_GUILD2, 38)]
    public class IZ_REQ_JOIN_GUILD2 : PacketBase
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public int ReqAID { get; set; }
        public string GuildName { get; set; }

        public IZ_REQ_JOIN_GUILD2()
        {
            Command = (ushort) PACKET_COMMAND.IZ_REQ_JOIN_GUILD2;
        }

        public IZ_REQ_JOIN_GUILD2(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    ReqAID = br.ReadInt32();
                    GuildName = br.ReadCString(24);

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
            bw.Write(GDID);
            bw.Write(AID);
            bw.Write(ReqAID);
            bw.WriteCString(GuildName, 24);
        }
    }
}
