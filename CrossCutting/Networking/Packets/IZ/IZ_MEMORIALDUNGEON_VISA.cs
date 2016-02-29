using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_MEMORIALDUNGEON_VISA, 26)]
    public class IZ_MEMORIALDUNGEON_VISA : PacketBase
    {
        public int GID { get; set; }
        public int AID { get; set; }
        public int ZSID { get; set; }
        public string MapName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public IZ_MEMORIALDUNGEON_VISA()
        {
            Command = (ushort) PACKET_COMMAND.IZ_MEMORIALDUNGEON_VISA;
        }

        public IZ_MEMORIALDUNGEON_VISA(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GID = br.ReadInt32();
                    AID = br.ReadInt32();
                    ZSID = br.ReadInt32();
                    MapName = br.ReadCString(16);
                    X = br.ReadInt32();
                    Y = br.ReadInt32();

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
            bw.Write(GID);
            bw.Write(AID);
            bw.Write(ZSID);
            bw.WriteCString(MapName, 16);
            bw.Write(X);
            bw.Write(Y);
        }
    }
}
