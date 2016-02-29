namespace Aegis.Logic.Management.Contracts.Player.Classes
{
    public class Player
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public int GDID { get; set; }
        public int GRID { get; set; }
        public string CharacterName { get; set; }
        public string AccountName { get; set; }
        public string MapName { get; set; }
        public int Level { get; set; }
        public string Memo { get; set; }
        public int Service { get; set; }
        public int MemberExp { get; set; }
        public int PositionID { get; set; }
        public short Head { get; set; }
        public short HeadPalette { get; set; }
        public short Sex { get; set; }
        public int Job { get; set; }
        public int Status { get; set; }
        public int ZSID { get; set; }
    }
}