using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_CHAR_LEVEL_UP)]
    public class ZI_CHAR_LEVEL_UP : PacketVarSize
    {
        public int GID { get; set; }
        public int AID { get; set; }
        public byte[] Data { get; set; }

        public ZI_CHAR_LEVEL_UP()
        {
            Command = (ushort) PACKET_COMMAND.ZI_CHAR_LEVEL_UP;
        }

        public ZI_CHAR_LEVEL_UP(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    GID = br.ReadInt32();
                    AID = br.ReadInt32();
                    Data = br.ReadBytes(Length - 12);

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
            bw.Write(Length);
            bw.Write(GID);
            bw.Write(AID);
            bw.Write(Data);
        }
    }
}
