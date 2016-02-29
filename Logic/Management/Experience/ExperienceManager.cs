using System.Collections.Generic;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.Data.Repositories.Contracts;
using Aegis.Logic.Management.Contracts.Experience;

namespace Aegis.Logic.Management.Experience
{
    public class ExperienceManager : IExperienceManager
    {
        private readonly IScriptRepository _scriptRepository;

        public ExperienceManager(IScriptRepository scriptRepository)
        {
            _scriptRepository = scriptRepository;
        }

        public IEnumerable<ExperienceInfo> GetGuildExp()
        {
            return _scriptRepository.GetGuildExp();
        }
    }
}
