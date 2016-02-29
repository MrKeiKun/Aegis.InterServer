using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_LIST_JOIN_GUILD_CHECK_GID, 14)]
    public class ZI_LIST_JOIN_GUILD_CHECK_GID : PacketBase
    {
        public int AID { get; set; }

        public ZI_LIST_JOIN_GUILD_CHECK_GID()
        {
            Command = (ushort) PACKET_COMMAND.ZI_LIST_JOIN_GUILD_CHECK_GID;
        }

        public ZI_LIST_JOIN_GUILD_CHECK_GID(byte[] packet) : base(packet)
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
