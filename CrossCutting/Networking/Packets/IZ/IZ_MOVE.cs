using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_MOVE, 62)]
    public class IZ_MOVE : PacketBase
    {
        public int MoveType { get; set; }
        public int SrcID { get; set; }
        public int DestID { get; set; }
        public int DestX { get; set; }
        public int DestY { get; set; }
        public string MapName { get; set; }
        public string CharName { get; set; }

        public IZ_MOVE()
        {
            Command = (ushort) PACKET_COMMAND.IZ_MOVE;
        }

        public IZ_MOVE(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    MoveType = br.ReadInt32();
                    SrcID = br.ReadInt32();
                    DestID = br.ReadInt32();
                    DestX = br.ReadInt32();
                    DestY = br.ReadInt32();
                    MapName = br.ReadCString(16);
                    CharName = br.ReadCString(24);

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
            bw.Write(DestID);
            bw.Write(DestX);
            bw.Write(DestY);
            bw.WriteCString(MapName, 16);
            bw.WriteCString(CharName, 24);
        }
    }
}
