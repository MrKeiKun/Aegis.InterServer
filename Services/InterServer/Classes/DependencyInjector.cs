using System;
using System.Linq;
using System.Reflection;
using Aegis.Infrastructure.Mappings;
using log4net;
using Ninject;

namespace Aegis.Services.InterServer.Classes
{
    public class DependencyInjector
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static volatile DependencyInjector _instance;
        private readonly StandardKernel _kernel;

        public static DependencyInjector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DependencyInjector();
                }

                return _instance;
            }
        }

        private DependencyInjector()
        {
            var mappings = Aggregator.Mappings.ToList();
            mappings.Add(new ServiceModule());
            _kernel = new StandardKernel(mappings.ToArray());
        }

        public IKernel GetKernel()
        {
            if (_kernel == null)
            {
                throw new ApplicationException("Kernel noch nicht erzeugt, bitte vor Verwendung der Get-Funktion den CTOR DependencyInjector.New(true/false) aufrufen; bei Testklassen z.B. als private readonly Member");
            }

            return _kernel;
        }

        public T Get<T>()
        {
            try
            {
                return GetKernel().Get<T>();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}