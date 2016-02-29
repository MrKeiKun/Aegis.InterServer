using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GUILD_ALLYINFO_TOD)]
    public class IZ_GUILD_ALLYINFO_TOD : PacketVarSize
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public GUILDALLYINFO[] GuildAllyInfo { get; set; }

        public IZ_GUILD_ALLYINFO_TOD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GUILD_ALLYINFO_TOD;
        }

        public IZ_GUILD_ALLYINFO_TOD(byte[] packet)
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
                    GuildAllyInfo = new GUILDALLYINFO[(Length - 12)/GUILDALLYINFO.Length];
                    for (var i = 0; i < GuildAllyInfo.Length; i++)
                    {
                        GuildAllyInfo[i] = new GUILDALLYINFO(br);
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

            for (var i = 0; i < GuildAllyInfo.Length; i++)
            {
                GuildAllyInfo[i].Write(bw);
            }
        }
    }
}