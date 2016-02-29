using Aegis.CrossCutting.Network.Packets;

namespace Aegis.Services.InterServer.Contracts.Interfaces
{
    public interface IInterServer
    {
        void Start();
        void Stop();
        void BroadcastPacket(PacketBase packet);
        void ZonePacket(int zsid, PacketBase packet);
        int GetLowestResourceZone();
    }
}
