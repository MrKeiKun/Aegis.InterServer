using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_ADD_EXP, 14)]
    public class ZI_ADD_EXP : PacketBase
    {
        public int GDID { get; set; }
        public int GID { get; set; }
        public int Exp { get; set; }

        public ZI_ADD_EXP()
        {
            Command = (ushort) PACKET_COMMAND.ZI_ADD_EXP;
        }

        public ZI_ADD_EXP(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Exp = br.ReadInt32();

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
            bw.Write(GID);
            bw.Write(Exp);
        }
    }
}
