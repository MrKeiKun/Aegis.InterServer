using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_ACK_LEAVE_GUILD, 78)]
    public class IZ_ACK_LEAVE_GUILD : PacketBase
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }
        public string CharName { get; set; }
        public string ReasonDesc { get; set; }

        public IZ_ACK_LEAVE_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_ACK_LEAVE_GUILD;
        }

        public IZ_ACK_LEAVE_GUILD(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    CharName = br.ReadCString(24);
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
            bw.WriteCString(CharName, 24);
            bw.WriteCString(ReasonDesc, 40);
        }
    }
}
