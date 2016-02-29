using Aegis.Data.Repositories;
using Aegis.Data.Repositories.Contracts;
using Ninject.Modules;

namespace Aegis.Infrastructure.Mappings
{
    internal class RepositoryMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ICharacterRepository>().To<CharacterRepository>().InSingletonScope();
            Bind<IIPInfoRepository>().To<IPInfoRepository>().InSingletonScope();
            Bind<IScriptRepository>().To<ScriptRepository>().InSingletonScope();
            Bind<IGlobalInfoRepository>().To<GlobalInfoRepository>().InSingletonScope();
        }
    }
}