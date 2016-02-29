using Aegis.CrossCutting.Network.Packets;
using Aegis.CrossCutting.Network.Packets.IZ;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Aegis.Services.InterServer.Classes
{
    public class ZoneClient : BaseClient
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int UsedKBytesMemory { get; set; }
        public int NumTotalNPC { get; set; }
        public int Sid { get; set; }
        private readonly Timer _pingTimer;

        public ZoneClient()
        {
            _pingTimer = new Timer { Interval = 15000, AutoReset = false };
            _pingTimer.Elapsed += pingTimer_Elapsed;
            //_pingTimer.Start();
        }

        public void EnqueuePacket(PacketBase packet)
        {
            this.Owner.Send(this, RagnarokListener.PacketToByte(packet));
        }

        private void pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _pingTimer.Start();
        }
    }
}
