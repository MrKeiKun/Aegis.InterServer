using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GUILDINFO_TOD, GUILDINFO.Length + 10)]
    public class IZ_GUILDINFO_TOD : PacketBase
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public GUILDINFO Data { get; set; }

        public IZ_GUILDINFO_TOD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GUILDINFO_TOD;
        }

        public IZ_GUILDINFO_TOD(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    Data = new GUILDINFO(br);

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
            bw.Write(GDID);
            bw.Write(AID);
            Data.Write(bw);
        }
    }
}