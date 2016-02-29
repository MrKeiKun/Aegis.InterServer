using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_GUILD_NOTICE, 186)]
    public class ZI_GUILD_NOTICE : PacketBase
    {
        public int GDID { get; set; }
        public string Subject { get; set; }
        public string Notice { get; set; }

        public ZI_GUILD_NOTICE()
        {
            Command = (ushort) PACKET_COMMAND.ZI_GUILD_NOTICE;
        }

        public ZI_GUILD_NOTICE(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    Subject = br.ReadCString(60);
                    Notice = br.ReadCString(120);

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
            bw.WriteCString(Subject, 60);
            bw.WriteCString(Notice, 120);
        }
    }
}