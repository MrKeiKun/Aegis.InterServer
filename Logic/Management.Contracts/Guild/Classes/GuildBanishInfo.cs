using System;

namespace Aegis.Logic.Management.Contracts.Guild.Classes
{
    public class GuildBanishInfo
    {
        public int GDID { get; set; }
        public string MemberName { get; set; }
        public string Reason { get; set; }
        public int GID { get; set; }
        public int AID { get; set; }
        public DateTime Date { get; set; }
    }
}