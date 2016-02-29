using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Aegis.CrossCutting.GlobalDataClasses;
using log4net;

namespace Aegis.Services.InterServer.Classes
{
    internal class ZServerConnector
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _port;
        private TcpListener _listener;
        public readonly ConcurrentList<ZServer> Zones;

        public ZServerConnector(int port)
        {
            _port = port;

            // concurrent...
            Zones = new ConcurrentList<ZServer>();
        }

        private static int _nextId = -1;

        public void Start()
        {
            try
            {
                _listener = new TcpListener(new IPEndPoint(IPAddress.Any, _port));
                _listener.Start();
                Logger.InfoFormat("Waiting on port {0} for zone/inter connections", _port);
                do
                {
                    while (!_listener.Pending())
                    {
                        if (ShouldStop)
                            return;
                    }

                    if (ShouldStop)
                        return;

                    var socket = _listener.AcceptSocket();
                    var nid = _nextId--;

                    var zone = DependencyInjector.Instance.Get<ZServer>();
                    zone.SId = nid;
                    zone.Socket = socket;
                    Zones.Add(zone);
                    new Thread(zone.Start) {Name = "Zone connection"}.Start();
                } while (true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }

        private bool ShouldStop
        {
            get
            {
                if (!InterServer.Instance.ServerShutdown.WaitOne(10)) return false;

                _listener.Stop();
                return true;
            }
        }
    }
}