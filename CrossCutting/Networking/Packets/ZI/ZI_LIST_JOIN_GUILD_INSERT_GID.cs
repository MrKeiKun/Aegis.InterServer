using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_LIST_JOIN_GUILD_INSERT_GID, 14)]
    public class ZI_LIST_JOIN_GUILD_INSERT_GID : PacketBase
    {
        public int GDID { get; set; }
        public int TargetAID { get; set; }
        public int TargetGID { get; set; }

        public ZI_LIST_JOIN_GUILD_INSERT_GID()
        {
            Command = (ushort) PACKET_COMMAND.ZI_LIST_JOIN_GUILD_INSERT_GID;
        }

        public ZI_LIST_JOIN_GUILD_INSERT_GID(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    TargetAID = br.ReadInt32();
                    TargetGID = br.ReadInt32();

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
            bw.Write(TargetAID);
            bw.Write(TargetGID);
        }
    }
}
