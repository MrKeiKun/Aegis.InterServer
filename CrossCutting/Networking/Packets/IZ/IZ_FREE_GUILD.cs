using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    /// <summary>
    /// This packet is sent to the zone servers after the last player of a guild has exited to tell the zoneserver to free resources.
    /// </summary>
    [Command(PACKET_COMMAND.IZ_FREE_GUILD, 6)]
    public class IZ_FREE_GUILD : PacketBase
    {
        public int GDID { get; set; }

        public IZ_FREE_GUILD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_FREE_GUILD;
        }

        public IZ_FREE_GUILD(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();

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
            bw.Write(GDID);
        }
    }
}