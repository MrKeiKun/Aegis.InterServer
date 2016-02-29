using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_ACK_WHISPER, 31)]
    public class ZI_ACK_WHISPER : PacketBase
    {
        public int SenderAID { get; set; }
        public string Sender { get; set; }
        public byte Result { get; set; }

        public ZI_ACK_WHISPER()
        {
            Command = (ushort) PACKET_COMMAND.ZI_ACK_WHISPER;
        }

        public ZI_ACK_WHISPER(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    SenderAID = br.ReadInt32();
                    Sender = br.ReadCString(24);
                    Result = br.ReadByte();

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
            bw.Write(SenderAID);
            bw.WriteCString(Sender, 24);
            bw.Write(Result);
        }
    }
}
