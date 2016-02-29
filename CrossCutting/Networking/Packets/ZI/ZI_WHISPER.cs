using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_WHISPER)]
    public class ZI_WHISPER : PacketVarSize
    {
        public int SenderAID { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string SenderAccountName { get; set; }
        public string Text { get; set; }

        public ZI_WHISPER()
        {
            Command = (ushort) PACKET_COMMAND.ZI_WHISPER;
        }

        public ZI_WHISPER(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    SenderAID = br.ReadInt32();
                    Sender = br.ReadCString(24);
                    Receiver = br.ReadCString(24);
                    SenderAccountName = br.ReadCString(24);
                    Text = br.ReadCString(Length - 80);

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
            bw.Write(SenderAID);
            bw.WriteCString(Sender, 24);
            bw.WriteCString(Receiver, 24);
            bw.WriteCString(SenderAccountName, 24);
            bw.WriteCString(Text);
        }
    }
}