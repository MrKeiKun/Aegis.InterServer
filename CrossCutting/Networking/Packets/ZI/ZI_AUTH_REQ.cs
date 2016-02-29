using System;
using System.IO;
using System.Text;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_AUTH_REQ, 12)]
    public class ZI_AUTH_REQ : PacketBase
    {
        public int IP { get; set; }
        public short Port { get; set; }
        public int ZSID { get; set; }

        public ZI_AUTH_REQ()
        {
            Command = (ushort)PACKET_COMMAND.ZI_AUTH_REQ;
        }

        public ZI_AUTH_REQ(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    IP = br.ReadInt32();
                    Port = br.ReadInt16();
                    ZSID = br.ReadInt32();

                    if (ms.Position != ms.Length)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        public override void WriteTo(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}