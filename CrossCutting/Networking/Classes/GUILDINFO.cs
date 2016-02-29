using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class GUILDINFO
    {
        public const int Length = 212;

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

        public GUILDINFO(int gdid, int level, string name, string mName, int maxUserNum, int userNum, int honor, int virtue, int type, int @class, int money, int arenaWin, int arenaLose, int arenaDrawn, string manageLand, int exp, int emblemVersion, int point, string desc)
        {
            GDID = gdid;
            Level = level;
            Name = name;
            MName = mName;
            MaxUserNum = maxUserNum;
            UserNum = userNum;
            Honor = honor;
            Virtue = virtue;
            Type = type;
            Class = @class;
            Money = money;
            ArenaWin = arenaWin;
            ArenaLose = arenaLose;
            ArenaDrawn = arenaDrawn;
            ManageLand = manageLand;
            Exp = exp;
            EmblemVersion = emblemVersion;
            Point = point;
            Desc = desc;
        }

        public GUILDINFO(BinaryReader br)
        {
            GDID = br.ReadInt32();
            Level = br.ReadInt32();
            Name = br.ReadCString(24);
            MName = br.ReadCString(24);
            MaxUserNum = br.ReadInt32();
            UserNum = br.ReadInt32();
            Honor = br.ReadInt32();
            Virtue = br.ReadInt32();
            Type = br.ReadInt32();
            Class = br.ReadInt32();
            Money = br.ReadInt32();
            ArenaWin = br.ReadInt32();
            ArenaLose = br.ReadInt32();
            ArenaDrawn = br.ReadInt32();
            ManageLand = br.ReadCString(24);
            Exp = br.ReadInt32();
            EmblemVersion = br.ReadInt32();
            Point = br.ReadInt32();
            Desc = br.ReadCString(80);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(GDID);
            bw.Write(Level);
            bw.WriteCString(Name, 24);
            bw.WriteCString(MName, 24);
            bw.Write(MaxUserNum);
            bw.Write(UserNum);
            bw.Write(Honor);
            bw.Write(Virtue);
            bw.Write(Type);
            bw.Write(Class);
            bw.Write(Money);
            bw.Write(ArenaWin);
            bw.Write(ArenaLose);
            bw.Write(ArenaDrawn);
            bw.WriteCString(ManageLand, 24);
            bw.Write(Exp);
            bw.Write(EmblemVersion);
            bw.Write(Point);
            bw.WriteCString(Desc, 80);
        }
    }
}