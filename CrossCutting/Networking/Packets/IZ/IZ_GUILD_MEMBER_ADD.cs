using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GUILD_MEMBER_ADD, 146)]
    public class IZ_GUILD_MEMBER_ADD : PacketBase
    {
        public int GDID { get; set; }
        public GUILDMINFO GUILDMINFO { get; set; }

        public IZ_GUILD_MEMBER_ADD()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GUILD_MEMBER_ADD;
        }

        public IZ_GUILD_MEMBER_ADD(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    GUILDMINFO = new GUILDMINFO(br);

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
            GUILDMINFO.Write(bw);
        }
    }
}
