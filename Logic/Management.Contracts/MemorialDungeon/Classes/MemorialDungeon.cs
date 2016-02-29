using System.Collections.Generic;

namespace Aegis.Logic.Management.Contracts.MemorialDungeon.Classes
{
    public class MemorialDungeon
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public int GRID { get; set; }
        public string DungeonName { get; set; }
        public int ZsId { get; set; }
        public IList<MemorialDungeonMap> Maps { get; set; }
    }
}
