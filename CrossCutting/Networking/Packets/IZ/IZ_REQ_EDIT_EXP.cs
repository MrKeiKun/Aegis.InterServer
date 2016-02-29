using System;
using System.IO;
using System.Text;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_REQ_EDIT_EXP, 18)]
    public class IZ_REQ_EDIT_EXP : PacketBase
    {
        public int MonitorNum { get; set; }
        public int Exp { get; set; }
        public int Death { get; set; }
        public int Drop { get; set; }

        public IZ_REQ_EDIT_EXP()
        {
            Command = (ushort)PACKET_COMMAND.IZ_REQ_EDIT_EXP;
        }

        public override void WriteTo(BinaryWriter bw)
        {
            bw.Write(Command);
            bw.Write(MonitorNum);
            bw.Write(Exp);
            bw.Write(Death);
            bw.Write(Drop);
        }
    }
}