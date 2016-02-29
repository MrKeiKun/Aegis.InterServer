using System;
using System.IO;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_UPDATE_CHARSTAT, 30)]
    public class IZ_UPDATE_CHARSTAT : PacketBase
    {
        public int GDID { get; set; }
        public int GID { get; set; }
        public int AID { get; set; }
        public int Status { get; set; }
        public short Sex { get; set; }
        public short Head { get; set; }
        public short HeadPalette { get; set; }
        public short Job { get; set; }
        public int Level { get; set; }

        public IZ_UPDATE_CHARSTAT()
        {
            Command = (ushort) PACKET_COMMAND.IZ_UPDATE_CHARSTAT;
        }

        public IZ_UPDATE_CHARSTAT(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    GDID = br.ReadInt32();
                    GID = br.ReadInt32();
                    AID = br.ReadInt32();
                    Status = br.ReadInt32();
                    Sex = br.ReadInt16();
                    Head = br.ReadInt16();
                    HeadPalette = br.ReadInt16();
                    Job = br.ReadInt16();
                    Level = br.ReadInt32();

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
            bw.Write(GID);
            bw.Write(AID);
            bw.Write(Status);
            bw.Write(Sex);
            bw.Write(Head);
            bw.Write(HeadPalette);
            bw.Write(Job);
            bw.Write(Level);
        }
    }
}