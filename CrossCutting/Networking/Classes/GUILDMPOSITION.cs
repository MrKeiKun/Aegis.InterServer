using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class GUILDMPOSITION
    {
        public const int Length = 48;

        public int GDID { get; set; }
        public int Grade { get; set; }
        public string PosName { get; set; }
        public int JoinRight { get; set; }
        public int PenaltyRight { get; set; }
        public int PositionID { get; set; }
        public int Service { get; set; }

        public GUILDMPOSITION(int gdid, int grade, string posName, int joinRight, int penaltyRight, int positionId, int service)
        {
            GDID = gdid;
            Grade = grade;
            PosName = posName;
            JoinRight = joinRight;
            PenaltyRight = penaltyRight;
            PositionID = positionId;
            Service = service;
        }

        public GUILDMPOSITION(BinaryReader br)
        {
            GDID = br.ReadInt32();
            Grade = br.ReadInt32();
            PosName = br.ReadCString(24);
            JoinRight = br.ReadInt32();
            PenaltyRight = br.ReadInt32();
            PositionID = br.ReadInt32();
            Service = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(GDID);
            bw.Write(Grade);
            bw.WriteCString(PosName, 24);
            bw.Write(JoinRight);
            bw.Write(PenaltyRight);
            bw.Write(PositionID);
            bw.Write(Service);
        }
    }
}