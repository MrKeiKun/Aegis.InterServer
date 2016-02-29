using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Packets;
using log4net;

namespace Aegis.CrossCutting.Network.Classes
{
    public class PacketBuffer
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ReaderWriterLock _rwl;
        private readonly BlockingCollection<PacketBase> _packets;
        private readonly CancellationToken _token;

        private byte[] _data = new byte[0];
        private int _length;
        private int _position;

        private int Length
        {
            get { return _length - _position; }
        }

        #region CTOR

        public PacketBuffer(CancellationToken token)
        {
            _position = 0;
            _rwl = new ReaderWriterLock();
            _packets = new BlockingCollection<PacketBase>();
            _token = token;
        }

        #endregion

        #region Public

        public PacketBase GetPacket()
        {
            if (Length < 2)
            {
                return null;
            }

            Type mit = null;
            byte[] packetData = null;
            _rwl.AcquireReaderLock(10000);
            packetData = null;
            packetData = _data;
            _rwl.ReleaseReaderLock();

            var command = (ushort)((packetData[1] << 8) | packetData[0]);
            if (!Enum.IsDefined(typeof(PACKET_COMMAND), command))
            {
                _position += 2;
                Consume();
                throw new BufferException("PacketBuffer::Dequeue()", string.Format("{0} (0x{0:X4}) packet is unknown", command), packetData);
            }

            var dataPos = 2;
            var packetInfo = PacketLengthManager.GetPacketInformation((PACKET_COMMAND)command);

            // read length from packet
            if (!packetInfo.HasValue)
            {
                packetInfo = (ushort)((packetData[dataPos + 1] << 8) | packetData[dataPos]);
            }

            // increase the position in reader buffer
            _position += packetInfo.Value;

            if ((packetData.Length < packetInfo.Value))
            {
                Logger.Debug(packetData.Hexdump());
                throw new BufferException("PacketBuffer::Dequeue()", string.Format("{0} (0x{1:X4}) wrong length {2}", ((PACKET_COMMAND)command), command, packetInfo), _data);
            }

            // resize packetData to real packet size
            if (packetData.Length > packetInfo.Value)
            {
                Array.Resize(ref packetData, packetInfo.Value);
            }

            mit = Command.GetType((PACKET_COMMAND)command);
            if (mit == null)
            {
                throw new BufferException("PacketBuffer::Dequeue()", "no parser", packetData);
            }

            try
            {
                Consume();
                return (PacketBase)Activator.CreateInstance(mit, packetData);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(NotImplementedException))
                {
                    Logger.ErrorFormat("Packet not implemented: {0}", mit.Name);
                    Logger.Debug(packetData.Hexdump());
                }
                else
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Appends new data to the ParsingQueue
        /// </summary>
        /// <param name="newdata">The received data</param>
        public void Append(byte[] newdata)
        {
            Append(newdata, newdata.Length);
        }

        #endregion

        #region Private

        private void Consume()
        {
            try
            {
                if (_position == 0)
                {
                    return;
                }

                var nb = new byte[Length];

                _rwl.AcquireWriterLock(10000);
                Array.Copy(_data, _position, nb, 0, Length);
                _data = nb;
                _rwl.ReleaseWriterLock();

                _length = _length - _position;
                _position = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
        }

        private void Append(byte[] newdata, int len)
        {
            _rwl.AcquireWriterLock(10000);
            if (_data.Length < _length + len)
                Array.Resize(ref _data, _length + len);

            Array.Copy(newdata, 0, _data, _length, len);
            _length = _length + len;

            _rwl.ReleaseWriterLock();
        }

        #endregion
    }
}