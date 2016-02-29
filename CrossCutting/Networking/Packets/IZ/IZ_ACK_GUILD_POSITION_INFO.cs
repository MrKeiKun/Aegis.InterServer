using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_ACK_GUILD_POSITION_INFO)]
    public class IZ_ACK_GUILD_POSITION_INFO : PacketVarSize
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public byte[] Data { get; set; }

        public IZ_ACK_GUILD_POSITION_INFO()
        {
            Command = (ushort) PACKET_COMMAND.IZ_ACK_GUILD_POSITION_INFO;
        }

        public IZ_ACK_GUILD_POSITION_INFO(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    Data = br.ReadBytes(Length - 12);

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
            bw.Write(Length);
            bw.Write(GDID);
            bw.Write(AID);
            bw.Write(Data);
        }
    }
}
