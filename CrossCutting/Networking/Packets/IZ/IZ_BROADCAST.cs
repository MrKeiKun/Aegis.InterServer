using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_BROADCAST)]
    public class IZ_BROADCAST : PacketVarSize
    {
        public string Text { get; set; }

        public IZ_BROADCAST()
        {
            Command = (ushort) PACKET_COMMAND.IZ_BROADCAST;
        }

        public IZ_BROADCAST(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    Text = br.ReadCString(Length - 4);

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
            bw.WriteCString(Text);
        }
    }
}