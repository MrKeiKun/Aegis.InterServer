using Aegis.CrossCutting.Network.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Aegis.Services.InterServer.Classes
{
    public class BaseClient
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public PacketBuffer Buffer;
        public RagnarokListener Owner;

        public IPAddress IP()
        {
            return ((IPEndPoint)workSocket.RemoteEndPoint).Address;
        }
    }
}
