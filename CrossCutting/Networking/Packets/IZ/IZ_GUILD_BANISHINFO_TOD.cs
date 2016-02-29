using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GUILD_BANISHINFO_TOD)]
    public class IZ_GUILD_BANISHINFO_TOD : PacketVarSize
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public GUILDBANISHINFO[] GuildBanishInfo { get; set; }

        public IZ_GUILD_BANISHINFO_TOD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GUILD_BANISHINFO_TOD;
        }

        public IZ_GUILD_BANISHINFO_TOD(byte[] packet)
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
                    GuildBanishInfo = new GUILDBANISHINFO[(Length - 12)/GUILDBANISHINFO.Length];
                    for (var i = 0; i < GuildBanishInfo.Length; i++)
                    {
                        GuildBanishInfo[i] = new GUILDBANISHINFO(br);
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

            for (var i = 0; i < GuildBanishInfo.Length; i++)
            {
                GuildBanishInfo[i].Write(bw);
            }
        }
    }
}