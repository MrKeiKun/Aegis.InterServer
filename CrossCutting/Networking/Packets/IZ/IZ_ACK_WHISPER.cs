using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_ACK_WHISPER, 7)]
    public class IZ_ACK_WHISPER : PacketBase
    {
        public int SenderAID { get; set; }
        public byte Result { get; set; }

        public IZ_ACK_WHISPER()
        {
            Command = (ushort) PACKET_COMMAND.IZ_ACK_WHISPER;
        }

        public IZ_ACK_WHISPER(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    SenderAID = br.ReadInt32();
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
            bw.Write(Result);
        }
    }
}