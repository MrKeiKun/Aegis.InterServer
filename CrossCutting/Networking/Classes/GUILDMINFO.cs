using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class GUILDMINFO
    {
        public const int Length = 140;

        public int GID { get; set; }
        public string CharacterName { get; set; }
        public string AccountName { get; set; }
        public int Level { get; set; }
        public string Memo { get; set; }
        public int Service { get; set; }
        public int MemberExp { get; set; }
        public int GDID { get; set; }
        public int AID { get; set; }
        public int PositionID { get; set; }
        public short Head { get; set; }
        public short HeadPalette { get; set; }
        public short Sex { get; set; }
        public int Job { get; set; }
        public int Status { get; set; }

        public GUILDMINFO(int gid, string characterName, string accountName, int level, string memo, int service, int memberExp, int gdid, int aid, int positionId, short head, short headPalette, short sex, int job, int status)
        {
            GID = gid;
            CharacterName = characterName;
            AccountName = accountName;
            Level = level;
            Memo = memo;
            Service = service;
            MemberExp = memberExp;
            GDID = gdid;
            AID = aid;
            PositionID = positionId;
            Head = head;
            HeadPalette = headPalette;
            Sex = sex;
            Job = job;
            Status = status;
        }

        public GUILDMINFO(BinaryReader br)
        {
            GID = br.ReadInt32();
            CharacterName = br.ReadCString(24);
            AccountName = br.ReadCString(24);
            Level = br.ReadInt32();
            Memo = br.ReadCString(50);
            Service = br.ReadInt32();
            MemberExp = br.ReadInt32();
            GDID = br.ReadInt32();
            AID = br.ReadInt32();
            PositionID = br.ReadInt32();
            Head = br.ReadInt16();
            HeadPalette = br.ReadInt16();
            Sex = br.ReadInt16();
            Job = br.ReadInt32();
            Status = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(GID);
            bw.WriteCString(CharacterName, 24);
            bw.WriteCString(AccountName, 24);
            bw.Write(Level);
            bw.WriteCString(Memo, 50);
            bw.Write(Service);
            bw.Write(MemberExp);
            bw.Write(GDID);
            bw.Write(AID);
            bw.Write(PositionID);
            bw.Write(Head);
            bw.Write(HeadPalette);
            bw.Write(Sex);
            bw.Write(Job);
            bw.Write(Status);
        }
    }
}