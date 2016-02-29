using System;
using System.IO;
using System.Text;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_AUTH_ACK, 2)]
    public class IZ_AUTH_ACK : PacketBase
    {
        public IZ_AUTH_ACK()
        {
            Command = (ushort)PACKET_COMMAND.IZ_AUTH_ACK;
        }

        public override void WriteTo(BinaryWriter bw)
        {
            bw.Write(Command);
        }
    }
}