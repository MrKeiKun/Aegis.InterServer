using System.Collections.Generic;
using Aegis.Data.Repositories.Contracts.Classes;

namespace Aegis.Data.Repositories.Contracts
{
    public interface IGlobalInfoRepository
    {
        IEnumerable<Server> GetServerInfo();
    }
}