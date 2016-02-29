using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_BAN_GUILD, 82)]
    public class ZI_REQ_BAN_GUILD : PacketBase
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }
        public int MyGID { get; set; }
        public string AccountName { get; set; }
        public string ReasonDesc { get; set; }

        public ZI_REQ_BAN_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_BAN_GUILD;
        }

        public ZI_REQ_BAN_GUILD(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    MyGID = br.ReadInt32();
                    AccountName = br.ReadCString(24);
                    ReasonDesc = br.ReadCString(40);

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
            bw.Write(GID);
            bw.Write(MyGID);
            bw.WriteCString(AccountName, 24);
            bw.WriteCString(ReasonDesc, 40);
        }
    }
}