using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_GRID_UPDATE, 14)]
    public class ZI_GRID_UPDATE : PacketBase
    {
        public int AID { get; set; }
        public int GRID { get; set; }
        public int ExpOption { get; set; }

        public ZI_GRID_UPDATE()
        {
            Command = (ushort) PACKET_COMMAND.ZI_GRID_UPDATE;
        }

        public ZI_GRID_UPDATE(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GRID = br.ReadInt32();
                    ExpOption = br.ReadInt32();

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
            bw.Write(GRID);
            bw.Write(ExpOption);
        }
    }
}