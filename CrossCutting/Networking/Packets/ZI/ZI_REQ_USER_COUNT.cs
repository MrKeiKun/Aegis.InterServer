using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_REQ_USER_COUNT, 6)]
    public class ZI_REQ_USER_COUNT : PacketBase
    {
        public int AID { get; set; }

        public ZI_REQ_USER_COUNT()
        {
            Command = (ushort) PACKET_COMMAND.ZI_REQ_USER_COUNT;
        }

        public ZI_REQ_USER_COUNT(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();

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
        }
    }
}