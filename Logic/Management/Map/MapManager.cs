using System.Collections.Generic;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.Data.Repositories.Contracts;
using Aegis.Logic.Management.Contracts.Map;

namespace Aegis.Logic.Management.Map
{
    public class MapManager : IMapManager
    {
        private readonly IIPInfoRepository _ipInfoRepository;

        public MapManager(IIPInfoRepository ipInfoRepository)
        {
            _ipInfoRepository = ipInfoRepository;
        }

        public IEnumerable<MapInfo> GetMapInfo()
        {
            return _ipInfoRepository.GetMapInfo();
        }
    }
}