using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_INSTANTMAP_PLAYER_ENTER3, 7)]
    public class IZ_INSTANTMAP_PLAYER_ENTER3 : PacketBase
    {
        public int MapId { get; set; }
        public bool PlayerEnter { get; set; }

        public IZ_INSTANTMAP_PLAYER_ENTER3()
        {
            Command = (ushort) PACKET_COMMAND.IZ_INSTANTMAP_PLAYER_ENTER3;
        }

        public IZ_INSTANTMAP_PLAYER_ENTER3(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    MapId = br.ReadInt32();
                    PlayerEnter = br.ReadBoolean();

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
            bw.Write(MapId);
            bw.Write(PlayerEnter);
        }
    }
}
