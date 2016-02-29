using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_RESULT_MAKE_GUILD, 11)]
    public class IZ_RESULT_MAKE_GUILD : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public byte Result { get; set; }

        public IZ_RESULT_MAKE_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_RESULT_MAKE_GUILD;
        }

        public IZ_RESULT_MAKE_GUILD(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Result = br.ReadByte();

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
            bw.Write(GID);
            bw.Write(Result);
        }
    }
}