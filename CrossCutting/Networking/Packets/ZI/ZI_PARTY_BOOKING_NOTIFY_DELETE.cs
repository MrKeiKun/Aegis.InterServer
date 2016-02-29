using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_PARTY_BOOKING_NOTIFY_DELETE, 6)]
    public class ZI_PARTY_BOOKING_NOTIFY_DELETE : PacketBase
    {
        public int Index { get; set; }

        public ZI_PARTY_BOOKING_NOTIFY_DELETE()
        {
            Command = (ushort) PACKET_COMMAND.ZI_PARTY_BOOKING_NOTIFY_DELETE;
        }

        public ZI_PARTY_BOOKING_NOTIFY_DELETE(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Index = br.ReadInt32();

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
            bw.Write(Index);
        }
    }
}
