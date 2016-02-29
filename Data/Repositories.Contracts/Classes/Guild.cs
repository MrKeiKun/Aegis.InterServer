using System.Collections.Generic;

namespace Aegis.Data.Repositories.Contracts.Classes
{
    public class Guild
    {
        public IEnumerable<GuildAllyInfo> GuildAllyInfo { get; set; }
        public IEnumerable<GuildBanishInfo> GuildBanishInfo { get; set; }
        public GuildInfo GuildInfo { get; set; }
        public IEnumerable<GuildMInfo> GuildMInfo { get; set; }
        public IEnumerable<GuildMPosition> GuildMPosition { get; set; }
        public GuildNotice GuildNotice { get; set; }
        public GuildSkill GuildSkill { get; set; }
    }
}