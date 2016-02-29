using System.Collections.Generic;
using Ninject.Modules;

namespace Aegis.Infrastructure.Mappings
{
    public static class Aggregator
    {
        public static IEnumerable<INinjectModule> Mappings => new INinjectModule[]
        {
            new ConfigurationMappings(),
            new ConfigurationStorageMappings(),
            new ManagerMappings(),
            new RepositoryMappings()
        };
    }
}