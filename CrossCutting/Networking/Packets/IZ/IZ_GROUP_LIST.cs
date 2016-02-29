using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_GROUP_LIST)]
    public class IZ_GROUP_LIST : PacketVarSize
    {
        public int AID { get; set; }
        public int ExpOption { get; set; }
        public string GroupName { get; set; }
        public CHARINFO_IN_GROUP[] CharinfoInGroup { get; set; }

        public IZ_GROUP_LIST()
        {
            Command = (ushort) PACKET_COMMAND.IZ_GROUP_LIST;
        }

        public IZ_GROUP_LIST(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Length = br.ReadUInt16();
                    AID = br.ReadInt32();
                    ExpOption = br.ReadInt32();
                    GroupName = br.ReadCString(24);
                    CharinfoInGroup = new CHARINFO_IN_GROUP[(Length - 36)/CHARINFO_IN_GROUP.Length];
                    for (var i = 0; i < CharinfoInGroup.Length; i++)
                    {
                        CharinfoInGroup[i] = new CHARINFO_IN_GROUP(br);
                    }

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
            bw.Write(AID);
            bw.Write(ExpOption);
            bw.WriteCString(GroupName, 24);

            for (var i = 0; i < CharinfoInGroup.Length; i++)
            {
                CharinfoInGroup[i].Write(bw);
            }
        }
    }
}