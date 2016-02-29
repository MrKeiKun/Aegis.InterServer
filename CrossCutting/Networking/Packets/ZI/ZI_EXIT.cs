using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_EXIT, 34)]
    public class ZI_EXIT : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public string CharacterName { get; set; }

        public ZI_EXIT()
        {
            Command = (ushort) PACKET_COMMAND.ZI_EXIT;
        }

        public ZI_EXIT(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    CharacterName = br.ReadCString(24);

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
            bw.WriteCString(CharacterName, 24);
        }
    }
}