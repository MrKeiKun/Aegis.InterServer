using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_MAKE_GUILD, 86)]
    public class ZI_REQ_MAKE_GUILD : PacketBase
    {
        public int GDID { get; set; }
        public int GID { get; set; }
        public int AID { get; set; }
        public string GName { get; set; }
        public string MName { get; set; }
        public string AccountName { get; set; }

        public ZI_REQ_MAKE_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_MAKE_GUILD;
        }

        public ZI_REQ_MAKE_GUILD(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    GID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GName = br.ReadCString(24);
                    MName = br.ReadCString(24);
                    AccountName = br.ReadCString(24);

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
            bw.Write(GID);
            bw.Write(AID);
            bw.WriteCString(GName, 24);
            bw.WriteCString(MName, 24);
            bw.WriteCString(AccountName, 24);
        }
    }
}