using Aegis.Services.InterServer.Contracts.Interfaces;
using log4net;
using Ninject.Modules;

namespace Aegis.Services.InterServer.Classes
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IInterServer>().To<InterServer>().InSingletonScope();
            Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target.Member.ReflectedType));
        }
    }
}