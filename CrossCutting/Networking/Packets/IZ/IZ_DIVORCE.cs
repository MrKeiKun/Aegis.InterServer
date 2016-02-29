using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_DIVORCE, 30)]
    public class IZ_DIVORCE : PacketBase
    {
        public int GID { get; set; }
        public string Name { get; set; }

        public IZ_DIVORCE()
        {
            Command = (ushort) PACKET_COMMAND.IZ_DIVORCE;
        }

        public IZ_DIVORCE(byte[] packet)
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
            bw.Write(Name);
        }
    }
}