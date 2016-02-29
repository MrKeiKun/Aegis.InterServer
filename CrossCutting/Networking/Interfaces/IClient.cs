namespace Aegis.CrossCutting.Network.Interfaces
{
    /// <summary>
    /// Client interface.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets or sets the TCP connection bound to client.
        /// </summary>
        IConnection Connection { get; set; }
    }
}
