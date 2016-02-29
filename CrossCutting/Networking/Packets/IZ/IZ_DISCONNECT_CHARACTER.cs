using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_DISCONNECT_CHARACTER, 6)]
    public class IZ_DISCONNECT_CHARACTER : PacketBase
    {
        public int AID { get; set; }

        public IZ_DISCONNECT_CHARACTER()
        {
            Command = (ushort) PACKET_COMMAND.IZ_DISCONNECT_CHARACTER;
        }

        public IZ_DISCONNECT_CHARACTER(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();

                    if (ms.Position != ms.Length)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        public override void WriteTo(BinaryWriter bw)
        {
            bw.Write(Command);
            bw.Write(AID);
        }
    }
}