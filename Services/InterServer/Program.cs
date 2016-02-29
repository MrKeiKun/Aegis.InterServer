using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Aegis.CrossCutting.Configuration.Contracts;
using Aegis.Services.InterServer.Classes;
using log4net;
using log4net.Config;
using Ninject;

[assembly: XmlConfigurator(Watch = true)]

namespace Aegis.Services.InterServer
{
    internal static class Program
    {
        public static Classes.InterServer InterServer;

        public static Thread InterServerThread;
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            XmlConfigurator.Configure();
            var config = DependencyInjector.Instance.GetKernel().Get<IConfigurator>();
            config.Load();

            Infrastructure.Mappings.AutoMapper.Setup();


            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                AssemblyCompanyAttribute company = null;
                if (attributes.Length > 0)
                {
                    company = attributes[0] as AssemblyCompanyAttribute;
                }

                if (company?.Company != "aegisdev.eu")
                {
                    continue;
                }

                Logger.Debug($"Assembly {assembly.GetName().Name}, Version {assembly.GetName().Version}");
            }
            StartGS();

            // testing
            //var guildManager = DependencyInjector.Instance.Get<IGuildManager>();
            //var guildInfo  = guildManager.GetGuild(12);
            
            while (true)
            {
                var line = Console.ReadLine();
            }
        }

        public static bool StartGS()
        {
            if (InterServer != null) return false;

            InterServer = new Classes.InterServer();
            InterServerThread = new Thread(InterServer.Run) { IsBackground = true,
                CurrentCulture = CultureInfo.InvariantCulture };
            InterServerThread.Start();

            return true;
        }

        public static bool StopGS()
        {
            if (InterServer == null) return false;

            Logger.Warn("Stopping Game-Server..");
            InterServerThread.Abort();
            InterServer = null;

            return true;
        }
    }
}