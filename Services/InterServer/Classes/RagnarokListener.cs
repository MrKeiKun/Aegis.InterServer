using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network;
using Aegis.CrossCutting.Network.Classes;
using Aegis.CrossCutting.Network.Packets;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aegis.Services.InterServer.Classes
{
    public abstract class RagnarokListener
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Thread signal.
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public RagnarokListener()
        {
        }

        public void StartListening(int port)
        {
            byte[] bytes = new Byte[1024];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            var source = new CancellationTokenSource();

            BaseClient client = CreateClient();
            client.Buffer = new PacketBuffer(source.Token);
            client.workSocket = handler;

            handler.BeginReceive(client.buffer, 0, BaseClient.BufferSize, 0, new AsyncCallback(ReadCallback), client);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            BaseClient state = (BaseClient)ar.AsyncState;
            Socket handler = state.workSocket;
            try
            {
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    var b = state.buffer;
                    Array.Resize(ref b, bytesRead);
                    state.Buffer.Append(b);

                    //Logger.Debug(b.Hexdump());

                    var packet = state.Buffer.GetPacket();
                    while (packet != null)
                    {
                        if (!OnPacket(state, packet))
                        {
                            RemoveClient(state, true);
                            return;
                        }

                        packet = state.Buffer.GetPacket();
                    }

                    handler.BeginReceive(state.buffer, 0, BaseClient.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                RemoveClient(state, true);
            }
        }

        public void Send(BaseClient handler, byte[] data)
        {
            handler.workSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        public void SendCallback(IAsyncResult ar)
        {
            BaseClient handler = (BaseClient)ar.AsyncState;
            try
            {
                int bytesSent = handler.workSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                RemoveClient(handler, true);
            }
        }

        public static byte[] PacketToByte(PacketBase packet)
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    packet.WriteTo(bw);
                    var packetInfo = PacketLengthManager.GetPacketInformation((PACKET_COMMAND)packet.Command);
                    if (packet.GetType().IsSubclassOf(typeof(PacketVarSize)))
                    {
                        ((PacketVarSize)packet).SetLength(bw);
                    }

                    var send = ms.ToArray();
                    if (!IsPacketLengthSane(packet, ms))
                    {
                        if (packetInfo != null)
                        {
                            Logger.ErrorFormat("Client::Send() {0} (0x{1:X4}) wrong length {2} != {3}", ((PACKET_COMMAND)packet.Command), packet.Command, ms.Position, packetInfo);
                        }
                        else
                        {
                            Logger.ErrorFormat("Client::Send() {0} (0x{1:X4}) wrong length {2}", ((PACKET_COMMAND)packet.Command), packet.Command, ms.Position);
                        }

                        throw new ApplicationException();
                    }

                    return send;
                }
            }
        }

        protected static bool IsPacketLengthSane(PacketBase packet, MemoryStream ms)
        {
            var info = PacketLengthManager.GetPacketInformation((PACKET_COMMAND)packet.Command);

            // Packets must have type (ushort) and sequence number (uint)
            if (ms.Length < 2)
            {
                return false;
            }

            if (info.HasValue && ms.Length != info.Value)
            {
                return false;
            }

            if (!info.HasValue)
            {
                var lenPos = 0;
                if (packet.GetType().IsSubclassOf(typeof(PacketVarSize)))
                {
                    lenPos = 2;
                }

                // Variable length packets must additioanlly have a length attribute (ushort)
                if (ms.Length < lenPos + 2)
                {
                    return false;
                }

                var offset = ms.Position;
                ms.Seek(lenPos, SeekOrigin.Begin);
                var size = ms.ReadByte() | (ms.ReadByte() << 8);
                ms.Seek(offset, SeekOrigin.Begin);

                if (size != ms.Length)
                {
                    return false;
                }
            }

            return true;
        }

        protected abstract BaseClient CreateClient();
        public abstract void RemoveClient(BaseClient client, bool notifyAccount);
        protected abstract bool OnPacket(BaseClient client, PacketBase packet);
    }
}
