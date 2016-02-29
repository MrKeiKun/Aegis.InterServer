using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aegis.CrossCutting.Network.Classes;
using log4net;
using log4net.Repository.Hierarchy;

namespace Aegis.Services.InterServer.Classes
{
    public sealed class InterServer
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ZoneListener ZoneListener;

        public void Run()
        {
            ZoneListener = DependencyInjector.Instance.Get<ZoneListener>();
            Task.Factory.StartNew(() => ZoneListener.StartListening(4001));
        }
    }
}
