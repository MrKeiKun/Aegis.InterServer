using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.ZI
{
    [Command(PACKET_COMMAND.ZI_LOGON, 86)]
    public class ZI_LOGON : PacketBase
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public int Sex { get; set; }
        public short Head { get; set; }
        public short HeadPalette { get; set; }
        public short Level { get; set; }
        public short Job { get; set; }
        public string AccountName { get; set; }
        public string CharacterName { get; set; }
        public string MapName { get; set; }

        public ZI_LOGON()
        {
            Command = (ushort) PACKET_COMMAND.ZI_LOGON;
        }

        public ZI_LOGON(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    AID = br.ReadInt32();
                    GID = br.ReadInt32();
                    Sex = br.ReadInt32();
                    Head = br.ReadInt16();
                    HeadPalette = br.ReadInt16();
                    Level = br.ReadInt16();
                    Job = br.ReadInt16();
                    AccountName = br.ReadCString(24);
                    CharacterName = br.ReadCString(24);
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
            bw.Write(AID);
            bw.Write(GID);
            bw.Write(Sex);
            bw.Write(Head);
            bw.Write(HeadPalette);
            bw.Write(Level);
            bw.Write(Job);
            bw.WriteCString(AccountName, 24);
            bw.WriteCString(CharacterName, 24);
            bw.WriteCString(MapName, 16);
        }
    }
}