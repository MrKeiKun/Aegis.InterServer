using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_MOVE, 42)]
    public class ZI_MOVE : PacketBase
    {
        public int MoveType { get; set; }
        public int SrcID { get; set; }
        public int DestX { get; set; }
        public int DestY { get; set; }
        public string DestName { get; set; }

        public ZI_MOVE()
        {
            Command = (ushort) PACKET_COMMAND.ZI_MOVE;
        }

        public ZI_MOVE(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    MoveType = br.ReadInt32();
                    SrcID = br.ReadInt32();
                    DestX = br.ReadInt32();
                    DestY = br.ReadInt32();
                    DestName = br.ReadCString(24);

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
            bw.Write(MoveType);
            bw.Write(SrcID);
            bw.Write(DestX);
            bw.Write(DestY);
            bw.WriteCString(DestName, 24);
        }
    }
}
