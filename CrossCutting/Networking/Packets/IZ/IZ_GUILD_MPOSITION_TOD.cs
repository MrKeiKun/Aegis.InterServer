using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GUILD_MPOSITION_TOD)]
    public class IZ_GUILD_MPOSITION_TOD : PacketVarSize
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public GUILDMPOSITION[] GuildMPosition { get; set; }

        public IZ_GUILD_MPOSITION_TOD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GUILD_MPOSITION_TOD;
        }

        public IZ_GUILD_MPOSITION_TOD(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
                    GuildMPosition = new GUILDMPOSITION[(Length - 12)/GUILDMPOSITION.Length];
                    for (var i = 0; i < GuildMPosition.Length; i++)
                    {
                        GuildMPosition[i] = new GUILDMPOSITION(br);
                    }

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

            for (var i = 0; i < GuildMPosition.Length; i++)
            {
                GuildMPosition[i].Write(bw);
            }
        }
    }
}