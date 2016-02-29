using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_ACK_LOGON, 14)]
    public class IZ_ACK_LOGON : PacketBase
    {
        public int Type { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }

        public IZ_ACK_LOGON()
        {
            Command = (ushort) PACKET_COMMAND.IZ_ACK_LOGON;
        }

        public IZ_ACK_LOGON(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Type = br.ReadInt32();
                    AID = br.ReadInt32();
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
            bw.Write(Type);
            bw.Write(AID);
            bw.Write(GID);
        }
    }
}