using System.Collections.Generic;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.Logic.Management.Contracts.Player
{
    public interface IPlayerManager
    {
        IEnumerable<Classes.Player> FindPlayersByAID(int aid);
        Classes.Player FindPlayerByGID(int gid);
        Classes.Player FindPlayerByName(string name);
        void AddPlayer(Classes.Player player);
        void FreePlayer(int aid);

        IEnumerable<MakerRank> GetTopMakerRank(int makerType);
    }
}