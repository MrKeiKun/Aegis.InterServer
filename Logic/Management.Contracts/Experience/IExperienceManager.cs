using System.Collections.Generic;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.Logic.Management.Contracts.Experience
{
    public interface IExperienceManager
    {
        IEnumerable<ExperienceInfo> GetGuildExp();
    }
}
