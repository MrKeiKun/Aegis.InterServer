using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_UPDATE_CHARGDID, 52)]
    public class IZ_UPDATE_CHARGDID : PacketBase
    {
        public byte Type { get; set; }
        public int GDID { get; set; }
        public int EmblemVer { get; set; }
        public int InterSID { get; set; }
        public int GID { get; set; }
        public int AID { get; set; }
        public int Right { get; set; }
        public bool IsMaster { get; set; }
        public string GuildName { get; set; }

        public IZ_UPDATE_CHARGDID()
        {
            Command = (ushort) PACKET_COMMAND.IZ_UPDATE_CHARGDID;
        }

        public IZ_UPDATE_CHARGDID(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Type = br.ReadByte();
                    GDID = br.ReadInt32();
                    EmblemVer = br.ReadInt32();
                    InterSID = br.ReadInt32();
                    GID = br.ReadInt32();
                    AID = br.ReadInt32();
                    Right = br.ReadInt32();
                    IsMaster = br.ReadBoolean();
                    GuildName = br.ReadCString(24);

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
            bw.Write(Type);
            bw.Write(GDID);
            bw.Write(EmblemVer);
            bw.Write(InterSID);
            bw.Write(GID);
            bw.Write(AID);
            bw.Write(Right);
            bw.Write(IsMaster);
            bw.WriteCString(GuildName, 24);
        }
    }
}