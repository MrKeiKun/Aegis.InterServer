using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_GUILD_CHANGEOWNER_AGIT, 26)]
    public class ZI_GUILD_CHANGEOWNER_AGIT : PacketBase
    {
        public int OldGDID { get; set; }
        public int NewGDID { get; set; }
        public string MapName { get; set; }

        public ZI_GUILD_CHANGEOWNER_AGIT()
        {
            Command = (ushort) PACKET_COMMAND.ZI_GUILD_CHANGEOWNER_AGIT;
        }

        public ZI_GUILD_CHANGEOWNER_AGIT(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    OldGDID = br.ReadInt32();
                    NewGDID = br.ReadInt32();
                    MapName = br.ReadCString(16);

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
            bw.Write(OldGDID);
            bw.Write(NewGDID);
            bw.WriteCString(MapName, 16);
        }
    }
}
