using System.Collections.Generic;

namespace Aegis.Logic.Management.Contracts.Guild.Classes
{
    public class GuildSkill
    {
        public int Point { get; set; }
        public IEnumerable<SkillEntry> Skills { get; set; }
    }
}