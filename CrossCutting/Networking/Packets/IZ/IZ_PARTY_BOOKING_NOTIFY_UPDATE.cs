using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_PARTY_BOOKING_NOTIFY_UPDATE, 18)]
    public class IZ_PARTY_BOOKING_NOTIFY_UPDATE : PacketBase
    {
        public int Index { get; set; }
        public short[] Job { get; set; }

        public IZ_PARTY_BOOKING_NOTIFY_UPDATE()
        {
            Command = (ushort)PACKET_COMMAND.IZ_PARTY_BOOKING_NOTIFY_UPDATE;
        }

        public IZ_PARTY_BOOKING_NOTIFY_UPDATE(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Index = br.ReadInt32();
                    Job = new short[6];
                    for (var i = 0; i < Job.Length; i++)
                    {
                        Job[i] = br.ReadInt16();
                    }

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
            for (var i = 0; i < Job.Length; i++)
            {
                bw.Write(Job[i]);
            }
        }
    }
}
