using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_CHAT_PARTY)]
    public class ZI_CHAT_PARTY : PacketVarSize
    {
        public int AID { get; set; }
        public int GRID { get; set; }
        public string Text { get; set; }

        public ZI_CHAT_PARTY()
        {
            Command = (ushort) PACKET_COMMAND.ZI_CHAT_PARTY;
        }

        public ZI_CHAT_PARTY(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GRID = br.ReadInt32();
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
            bw.Write(AID);
            bw.Write(GRID);
            bw.WriteCString(Text);
        }
    }
}