using System;
using System.IO;
using System.Text;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_PING_LIVE, 2)]
    public class IZ_PING_LIVE : PacketBase
    {
        public IZ_PING_LIVE()
        {
            Command = (ushort)PACKET_COMMAND.IZ_PING_LIVE;
        }

        public override void WriteTo(BinaryWriter bw)
        {
            bw.Write(Command);
        }
    }
}