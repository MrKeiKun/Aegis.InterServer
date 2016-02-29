using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GUILD_MAP_CHANGE, 26)]
    public class IZ_GUILD_MAP_CHANGE : PacketBase
    {
        public int GDID { get; set; }
        public int AID { get; set; }
        public string MapName { get; set; }

        public IZ_GUILD_MAP_CHANGE()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GUILD_MAP_CHANGE;
        }

        public IZ_GUILD_MAP_CHANGE(byte[] packet) : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    AID = br.ReadInt32();
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
            bw.Write(GDID);
            bw.Write(AID);
            bw.WriteCString(MapName, 16);
        }
    }
}
