using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_INSTANTMAP_CREATE_REQ, 30)]
    public class IZ_INSTANTMAP_CREATE_REQ : PacketBase
    {
        public int RequestN2Obj { get; set; }
        public string MapName { get; set; }
        public int MapId { get; set; }
        public int MapType { get; set; }

        public IZ_INSTANTMAP_CREATE_REQ()
        {
            Command = (ushort) PACKET_COMMAND.IZ_INSTANTMAP_CREATE_REQ;
        }

        public IZ_INSTANTMAP_CREATE_REQ(byte[] packet)
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
                    MapType = br.ReadInt32();

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
            bw.Write(MapType);
        }
    }
}
