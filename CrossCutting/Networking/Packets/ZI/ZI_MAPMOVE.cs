using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_MAPMOVE, 22)]
    public class ZI_MAPMOVE : PacketBase
    {
        public int AID { get; set; }
        public string MapName { get; set; }

        public ZI_MAPMOVE()
        {
            Command = (ushort) PACKET_COMMAND.ZI_MAPMOVE;
        }

        public ZI_MAPMOVE(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
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
            bw.Write(AID);
            bw.WriteCString(MapName, 16);
        }
    }
}