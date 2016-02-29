using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_GUILD_CHANGEMAXUSER, 10)]
    public class ZI_GUILD_CHANGEMAXUSER : PacketBase
    {
        public int GDID { get; set; }
        public int MaxNum { get; set; }

        public ZI_GUILD_CHANGEMAXUSER()
        {
            Command = (ushort) PACKET_COMMAND.ZI_GUILD_CHANGEMAXUSER;
        }

        public ZI_GUILD_CHANGEMAXUSER(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    MaxNum = br.ReadInt32();

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
            bw.Write(MaxNum);
        }
    }
}
