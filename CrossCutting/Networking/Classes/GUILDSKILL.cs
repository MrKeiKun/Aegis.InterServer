using System.IO;

namespace Aegis.CrossCutting.Network.Classes
{
    public class GUILDSKILL
    {
        public const int Length = 4;

        public short SkillId { get; set; }
        public short Level { get; set; }

        public GUILDSKILL(short skillId, short level)
        {
            SkillId = skillId;
            Level = level;
        }

        public GUILDSKILL(BinaryReader br)
        {
            SkillId = br.ReadInt16();
            Level = br.ReadInt16();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(SkillId);
            bw.Write(Level);
        }
    }
}