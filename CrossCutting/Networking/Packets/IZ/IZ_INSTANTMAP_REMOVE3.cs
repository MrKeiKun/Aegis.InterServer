using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_INSTANTMAP_REMOVE3, 26)]
    public class IZ_INSTANTMAP_REMOVE3 : PacketBase
    {
        public int ZSID { get; set; }
        public int MapId { get; set; }
        public string MapName { get; set; }

        public IZ_INSTANTMAP_REMOVE3()
        {
            Command = (ushort) PACKET_COMMAND.IZ_INSTANTMAP_REMOVE3;
        }

        public IZ_INSTANTMAP_REMOVE3(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    ZSID = br.ReadInt32();
                    MapId = br.ReadInt32();
                    MapName = br.ReadCString(16);

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
            bw.Write(ZSID);
            bw.Write(MapId);
            bw.WriteCString(MapName, 16);
        }
    }
}
