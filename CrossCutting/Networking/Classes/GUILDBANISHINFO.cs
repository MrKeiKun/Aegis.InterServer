using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class GUILDBANISHINFO
    {
        public const int Length = 110;

        public int GDID { get; set; }
        public string MemberName { get; set; }
        public string MemberAccount { get; set; }
        public string Reason { get; set; }
        public int GID { get; set; }
        public int AID { get; set; }

        public GUILDBANISHINFO(int gdid, string memberName, string memberAccount, string reason, int gid, int aid)
        {
            GDID = gdid;
            MemberName = memberName;
            MemberAccount = memberAccount;
            Reason = reason;
            GID = gid;
            AID = aid;
        }

        public GUILDBANISHINFO(BinaryReader br)
        {
            GDID = br.ReadInt32();
            MemberName = br.ReadCString(24);
            MemberAccount = br.ReadCString(24);
            Reason = br.ReadCString(50);
            GID = br.ReadInt32();
            AID = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(GDID);
            bw.WriteCString(MemberName, 24);
            bw.WriteCString(MemberAccount, 24);
            bw.WriteCString(Reason, 50);
            bw.Write(GID);
            bw.Write(AID);
        }
    }
}