using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_INSTANTMAP_ADD3, 31)]
    public class IZ_INSTANTMAP_ADD3 : PacketBase
    {
        public int ZSID { get; set; }
        public string MapName { get; set; }
        public int MapId { get; set; }
        public int MapType { get; set; }
        public bool PlayerEnter { get; set; }

        public IZ_INSTANTMAP_ADD3()
        {
            Command = (ushort) PACKET_COMMAND.IZ_INSTANTMAP_ADD3;
        }

        public IZ_INSTANTMAP_ADD3(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    ZSID = br.ReadInt32();
                    MapName = br.ReadCString(16);
                    MapId = br.ReadInt32();
                    MapType = br.ReadInt32();
                    PlayerEnter = br.ReadBoolean();

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
            bw.WriteCString(MapName, 16);
            bw.Write(MapId);
            bw.Write(MapType);
            bw.Write(PlayerEnter);
        }
    }
}
