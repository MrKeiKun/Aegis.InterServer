using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class CHARINFO_IN_GROUP
    {
        public const int Length = 46;

        public int AID { get; set; }
        public string CharacterName { get; set; }
        public string MapName { get; set; }
        public byte Role { get; set; }
        public byte CurState { get; set; }

        public CHARINFO_IN_GROUP(int aid, string characterName, string mapName, byte role, byte curState)
        {
            AID = aid;
            CharacterName = characterName;
            MapName = mapName;
            Role = role;
            CurState = curState;
        }

        public CHARINFO_IN_GROUP(BinaryReader br)
        {
            AID = br.ReadInt32();
            CharacterName = br.ReadCString(24);
            MapName = br.ReadCString(16);
            Role = br.ReadByte();
            CurState = br.ReadByte();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(AID);
            bw.WriteCString(CharacterName, 24);
            bw.WriteCString(MapName, 16);
            bw.Write(Role);
            bw.Write(CurState);
        }
    }
}