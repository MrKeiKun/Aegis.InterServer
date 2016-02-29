using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_JOIN_GUILD2, 34)]
    public class ZI_REQ_JOIN_GUILD2 : PacketBase
    {
        public int MyAID { get; set; }
        public int MyGID { get; set; }
        public string ReceiverName { get; set; }

        public ZI_REQ_JOIN_GUILD2()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_JOIN_GUILD2;
        }

        public ZI_REQ_JOIN_GUILD2(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    MyAID = br.ReadInt32();
                    MyGID = br.ReadInt32();
                    ReceiverName = br.ReadCString(24);

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
            bw.Write(MyAID);
            bw.Write(MyGID);
            bw.WriteCString(ReceiverName, 24);
        }
    }
}
