using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_STATEINFO, 10)]
    public class ZI_STATEINFO : PacketBase
    {
        public int UsedKBytesMemory { get; set; }
        public int NumTotalNPC { get; set; }

        public ZI_STATEINFO()
        {
            Command = (ushort) PACKET_COMMAND.ZI_STATEINFO;
        }

        public ZI_STATEINFO(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    UsedKBytesMemory = br.ReadInt32();
                    NumTotalNPC = br.ReadInt32();

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
            bw.Write(UsedKBytesMemory);
            bw.Write(NumTotalNPC);
        }
    }
}