using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_JOIN_GUILD, 66)]
    public class ZI_JOIN_GUILD : PacketBase
    {
        public int GDID { get; set; }
        public string Name { get; set; }
        public string AccountName { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }
        public int Answer { get; set; }

        public ZI_JOIN_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.ZI_JOIN_GUILD;
        }

        public ZI_JOIN_GUILD(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    Name = br.ReadCString(24);
                    AccountName = br.ReadCString(24);
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Answer = br.ReadInt32();

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
            bw.WriteCString(Name, 24);
            bw.WriteCString(AccountName, 24);
            bw.Write(AID);
            bw.Write(GID);
            bw.Write(Answer);
        }
    }
}
