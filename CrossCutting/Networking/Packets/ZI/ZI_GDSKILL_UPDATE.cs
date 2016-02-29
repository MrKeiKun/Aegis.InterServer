using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_GDSKILL_UPDATE)]
    public class ZI_GDSKILL_UPDATE : PacketVarSize
    {
        public int GDID { get; set; }
        public int SkillPoint { get; set; }
        public GUILDSKILL[] GuildSkill { get; set; }

        public ZI_GDSKILL_UPDATE()
        {
            Command = (ushort)PACKET_COMMAND.ZI_GDSKILL_UPDATE;
        }

        public ZI_GDSKILL_UPDATE(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    SkillPoint = br.ReadInt32();

                    GuildSkill = new GUILDSKILL[(Length - 12) / GUILDSKILL.Length];
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
            bw.Write(SkillPoint);

            for (var i = 0; i < GuildSkill.Length; i++)
            {
                GuildSkill[i].Write(bw);
            }
        }
    }
}