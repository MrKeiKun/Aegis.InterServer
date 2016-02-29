using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_UPDATE_MAKERRANK, 36)]
    public class ZI_UPDATE_MAKERRANK : PacketBase
    {
        public short Type { get; set; }
        public int GID { get; set; }
        public int Point { get; set; }
        public string CharacterName { get; set; }

        public ZI_UPDATE_MAKERRANK()
        {
            Command = (ushort) PACKET_COMMAND.ZI_UPDATE_MAKERRANK;
        }

        public ZI_UPDATE_MAKERRANK(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Type = br.ReadInt16();
                    GID = br.ReadInt32();
                    Point = br.ReadInt32();
                    CharacterName = br.ReadCString(24);

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
            bw.Write(GID);
            bw.Write(Point);
            bw.WriteCString(CharacterName, 24);
        }
    }
}
