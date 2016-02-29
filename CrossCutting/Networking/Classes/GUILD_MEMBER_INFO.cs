using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class GUILD_MEMBER_INFO
    {
        public const int Length = 104;

        public int AID { get; set; }
        public int GID { get; set; }
        public short Head { get; set; }
        public short HeadPalette { get; set; }
        public short Sex { get; set; }
        public short Job { get; set; }
        public short Level { get; set; }
        public int ContributionExp { get; set; }
        public int CurrentState { get; set; }
        public int PositionID { get; set; }
        public string Intro { get; set; }
        public string CharacterName { get; set; }

        public GUILD_MEMBER_INFO(BinaryReader br)
        {
            AID = br.ReadInt32();
            GID = br.ReadInt32();
            Head = br.ReadInt16();
            HeadPalette = br.ReadInt16();
            Sex = br.ReadInt16();
            Job = br.ReadInt16();
            Level = br.ReadInt16();
            ContributionExp = br.ReadInt32();
            CurrentState = br.ReadInt32();
            PositionID = br.ReadInt32();
            Intro = br.ReadCString(50);
            CharacterName = br.ReadCString(24);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(AID);
            bw.Write(GID);
            bw.Write(Head);
            bw.Write(HeadPalette);
            bw.Write(Sex);
            bw.Write(Job);
            bw.Write(Level);
            bw.Write(ContributionExp);
            bw.Write(CurrentState);
            bw.Write(PositionID);
            bw.WriteCString(Intro, 50);
            bw.WriteCString(CharacterName, 24);
        }
    }
}