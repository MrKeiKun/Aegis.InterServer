using Aegis.CrossCutting.ConfigurationStore;
using Aegis.CrossCutting.ConfigurationStore.Contracts;
using Ninject.Modules;

namespace Aegis.Infrastructure.Mappings
{
    internal class ConfigurationStorageMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigurationStore>().To<JsonConfigurationStore>().WithConstructorArgument("pathToJsonFile", "config.json");
        }
    }
}