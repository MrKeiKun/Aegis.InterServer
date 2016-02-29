using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_PARTY_BOOKING_NOTIFY_INSERT, 54)]
    public class ZI_PARTY_BOOKING_NOTIFY_INSERT : PacketBase
    {
        public int GID { get; set; }
        public PARTY_BOOKING_AD_INFO Info { get; set; }

        public ZI_PARTY_BOOKING_NOTIFY_INSERT()
        {
            Command = (ushort) PACKET_COMMAND.ZI_PARTY_BOOKING_NOTIFY_INSERT;
        }

        public ZI_PARTY_BOOKING_NOTIFY_INSERT(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GID = br.ReadInt32();
                    Info = new PARTY_BOOKING_AD_INFO(br);

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
            bw.Write(GID);
            Info.Write(bw);
        }
    }
}
