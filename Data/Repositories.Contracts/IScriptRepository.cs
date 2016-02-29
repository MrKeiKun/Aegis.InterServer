using System.Collections.Generic;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.Data.Repositories.Contracts
{
    public interface IScriptRepository
    {
        IEnumerable<ExperienceInfo> GetGuildExp();
    }
}