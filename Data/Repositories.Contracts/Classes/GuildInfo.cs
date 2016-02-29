namespace Aegis.Data.Repositories.Contracts.Classes
{
    public class GuildInfo
    {
        public int GDID { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string MName { get; set; }
        public int MaxUserNum { get; set; }
        public int UserNum { get; set; }
        public int Honor { get; set; }
        public int Virtue { get; set; }
        public int Type { get; set; }
        public int Class { get; set; }
        public int Money { get; set; }
        public int ArenaWin { get; set; }
        public int ArenaLose { get; set; }
        public int ArenaDrawn { get; set; }
        public string ManageLand { get; set; }
        public int Exp { get; set; }
        public int EmblemVersion { get; set; }
        public int Point { get; set; }
        public string Desc { get; set; }
    }
}