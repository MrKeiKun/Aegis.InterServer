using System.Collections.Generic;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.Logic.Management.Contracts.Map
{
    public interface IMapManager
    {
        IEnumerable<MapInfo> GetMapInfo();
    }
}