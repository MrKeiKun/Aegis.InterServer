using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GUILD_NOTIFYSKILLDATA)]
    public class IZ_GUILD_NOTIFYSKILLDATA : PacketVarSize
    {
        public int GDID { get; set; }
        public int IsForceSend { get; set; }
        public int SkillPoint { get; set; }
        public GUILDSKILL[] GuildSkill { get; set; }

        public IZ_GUILD_NOTIFYSKILLDATA()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GUILD_NOTIFYSKILLDATA;
        }

        public IZ_GUILD_NOTIFYSKILLDATA(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    IsForceSend = br.ReadInt32();
                    SkillPoint = br.ReadInt32();
                    GuildSkill = new GUILDSKILL[(Length - 14)/GUILDSKILL.Length];
                    for (var i = 0; i < GuildSkill.Length; i++)
                    {
                        GuildSkill[i] = new GUILDSKILL(br);
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
            bw.Write(IsForceSend);
            bw.Write(SkillPoint);

            for (var i = 0; i < GuildSkill.Length; i++)
            {
                GuildSkill[i].Write(bw);
            }
        }
    }
}