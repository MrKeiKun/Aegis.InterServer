using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_DIVORCE, 30)]
    public class ZI_DIVORCE : PacketBase
    {
        public int GID { get; set; }
        public string Name { get; set; }

        public ZI_DIVORCE()
        {
            Command = (ushort) PACKET_COMMAND.ZI_DIVORCE;
        }

        public ZI_DIVORCE(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GID = br.ReadInt32();
                    Name = br.ReadCString(24);

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
            bw.WriteCString(Name, 24);
        }
    }
}