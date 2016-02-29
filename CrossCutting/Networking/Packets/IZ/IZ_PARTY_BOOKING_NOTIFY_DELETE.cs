﻿using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_PARTY_BOOKING_NOTIFY_DELETE, 6)]
    public class IZ_PARTY_BOOKING_NOTIFY_DELETE : PacketBase
    {
        public int Index { get; set; }

        public IZ_PARTY_BOOKING_NOTIFY_DELETE()
        {
            Command = (ushort) PACKET_COMMAND.IZ_PARTY_BOOKING_NOTIFY_DELETE;
        }

        public IZ_PARTY_BOOKING_NOTIFY_DELETE(byte[] packet) : base(packet)
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
