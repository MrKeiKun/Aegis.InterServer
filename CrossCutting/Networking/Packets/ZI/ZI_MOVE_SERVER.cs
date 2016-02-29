using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_MOVE_SERVER, 34)]
    public class ZI_MOVE_SERVER : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public string CharName { get; set; }

        public ZI_MOVE_SERVER()
        {
            Command = (ushort) PACKET_COMMAND.ZI_MOVE_SERVER;
        }

        public ZI_MOVE_SERVER(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
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
            bw.Write(AID);
            bw.Write(GID);
            bw.WriteCString(CharName, 24);
        }
    }
}