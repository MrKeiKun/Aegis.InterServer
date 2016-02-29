using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_REQ_DISORGANIZE_GUILD_RESULT, 18)]
    public class IZ_REQ_DISORGANIZE_GUILD_RESULT : PacketBase
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public int GID { get; set; }
        public int Result { get; set; }

        public IZ_REQ_DISORGANIZE_GUILD_RESULT()
        {
            Command = (ushort) PACKET_COMMAND.IZ_REQ_DISORGANIZE_GUILD_RESULT;
        }

        public IZ_REQ_DISORGANIZE_GUILD_RESULT(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Result = br.ReadInt32();

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
            bw.Write(GID);
            bw.Write(Result);
        }
    }
}
