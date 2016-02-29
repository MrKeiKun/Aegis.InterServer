using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_INSTANTMAP_CREATE_RES, 27)]
    public class ZI_INSTANTMAP_CREATE_RES : PacketBase
    {
        public int RequestN2Obj { get; set; }
        public string MapName { get; set; }
        public int MapId { get; set; }
        public bool Success { get; set; }

        public ZI_INSTANTMAP_CREATE_RES()
        {
            Command = (ushort) PACKET_COMMAND.ZI_INSTANTMAP_CREATE_RES;
        }

        public ZI_INSTANTMAP_CREATE_RES(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    RequestN2Obj = br.ReadInt32();
                    MapName = br.ReadCString(16);
                    MapId = br.ReadInt32();
                    Success = br.ReadBoolean();

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
            bw.Write(RequestN2Obj);
            bw.WriteCString(MapName, 16);
            bw.Write(MapId);
            bw.Write(Success);
        }
    }
}
