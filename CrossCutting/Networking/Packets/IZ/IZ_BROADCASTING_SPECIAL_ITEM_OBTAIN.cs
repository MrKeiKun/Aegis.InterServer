using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_BROADCASTING_SPECIAL_ITEM_OBTAIN)]
    public class IZ_BROADCASTING_SPECIAL_ITEM_OBTAIN : PacketVarSize
    {
        public byte Type { get; set; }
        public short ItemID { get; set; }
        public string Text { get; set; }

        public IZ_BROADCASTING_SPECIAL_ITEM_OBTAIN()
        {
            Command = (ushort) PACKET_COMMAND.IZ_BROADCASTING_SPECIAL_ITEM_OBTAIN;
        }

        public IZ_BROADCASTING_SPECIAL_ITEM_OBTAIN(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    Type = br.ReadByte();
                    ItemID = br.ReadInt16();
                    Text = br.ReadCString(Length - 7);

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
            bw.Write(Type);
            bw.Write(ItemID);
            bw.WriteCString(Text);
        }
    }
}
