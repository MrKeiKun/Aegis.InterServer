using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Aegis.CrossCutting.Network.Packets;

namespace Aegis.CrossCutting.Network.Interfaces
{
    /// <summary>
    /// Connection interface.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// Returns true if there exists an active connection.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Returns remote endpoint.
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Returns local endpoint.
        /// </summary>
        IPEndPoint LocalEndPoint { get; }

        /// <summary>
        /// Gets or sets bound client.
        /// </summary>
        IClient Client { get; set; }

        /// <summary>
        /// Gets underlying socket.
        /// </summary>
        Socket Socket { get; }

        /// <summary>
        /// Sends a <see cref="PacketOut"/> to remote endpoint.
        /// </summary>
        /// <param name="packet"><see cref="PacketOut"/> to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(PacketBase packet);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer, SocketFlags flags);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="start">Start index to read from buffer.</param>
        /// <param name="count">Count of bytes to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer, int start, int count);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="start">Start index to read from buffer.</param>
        /// <param name="count">Count of bytes to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer, int start, int count, SocketFlags flags);

        /// <summary>
        /// Sends an enumarable byte buffer to remote endpoint.
        /// </summary>
        /// <param name="data">Enumrable byte buffer to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(IEnumerable<byte> data);

        /// <summary>
        /// Sends an enumarable byte buffer to remote endpoint.
        /// </summary>
        /// <param name="data">Enumrable byte buffer to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(IEnumerable<byte> data, SocketFlags flags);

        /// <summary>
        /// Kills the connection to remote endpoint.
        /// </summary>
        void Disconnect();
    }
}
