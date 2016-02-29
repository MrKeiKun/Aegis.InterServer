using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class GUILDALLYINFO
    {
        public const int Length = 36;

        public int GDID { get; set; }
        public int OpponentGDID { get; set; }
        public string GuildName { get; set; }
        public int Relation { get; set; }

        public GUILDALLYINFO(int gdid, int opponentGdid, string guildName, int relation)
        {
            GDID = gdid;
            OpponentGDID = opponentGdid;
            GuildName = guildName;
            Relation = relation;
        }

        public GUILDALLYINFO(BinaryReader br)
        {
            GDID = br.ReadInt32();
            OpponentGDID = br.ReadInt32();
            GuildName = br.ReadCString(24);
            Relation = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(GDID);
            bw.Write(OpponentGDID);
            bw.WriteCString(GuildName, 24);
            bw.Write(Relation);
        }
    }
}