using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_INSTNATMAP_DESTROY, 22)]
    public class IZ_INSTNATMAP_DESTROY : PacketBase
    {
        public string MapName { get; set; }
        public int MapId { get; set; }

        public IZ_INSTNATMAP_DESTROY()
        {
            Command = (ushort) PACKET_COMMAND.IZ_INSTNATMAP_DESTROY;
        }

        public IZ_INSTNATMAP_DESTROY(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    MapName = br.ReadCString(16);
                    MapId = br.ReadInt32();

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
            bw.WriteCString(MapName, 16);
            bw.Write(MapId);
        }
    }
}
