using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_JOIN_GUILD, 18)]
    public class ZI_REQ_JOIN_GUILD : PacketBase
    {
        public int AID { get; set; }
        public int MyGID { get; set; }
        public int MyAID { get; set; }
        public int GID { get; set; }

        public ZI_REQ_JOIN_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_JOIN_GUILD;
        }

        public ZI_REQ_JOIN_GUILD(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    MyGID = br.ReadInt32();
                    MyAID = br.ReadInt32();
                    GID = br.ReadInt32();

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
            bw.Write(MyGID);
            bw.Write(MyAID);
            bw.Write(GID);
        }
    }
}
