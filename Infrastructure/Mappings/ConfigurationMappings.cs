using Aegis.CrossCutting.Configuration;
using Aegis.CrossCutting.Configuration.Contracts;
using Ninject.Modules;

namespace Aegis.Infrastructure.Mappings
{
    internal class ConfigurationMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigurator>().To<Configurator>().InSingletonScope();
        }
    }
}