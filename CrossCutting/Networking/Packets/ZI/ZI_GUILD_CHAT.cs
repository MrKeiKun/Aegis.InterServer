using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_GUILD_CHAT)]
    public class ZI_GUILD_CHAT : PacketVarSize
    {
        public int GDID { get; set; }
        public string Text { get; set; }

        public ZI_GUILD_CHAT()
        {
            Command = (ushort) PACKET_COMMAND.ZI_GUILD_CHAT;
        }

        public ZI_GUILD_CHAT(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    Text = br.ReadCString(Length - 8);

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
            bw.Write(Length);
            bw.Write(GDID);
            bw.WriteCString(Text);
        }
    }
}