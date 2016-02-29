using System;
using System.IO;
using Aegis.CrossCutting.GlobalDataClasses;
using Aegis.CrossCutting.Network.Classes;

namespace Aegis.CrossCutting.Network.Packets.IZ
{
    [Command(PACKET_COMMAND.IZ_NOTIFY_MAKERRANK, 324)]
    public class IZ_NOTIFY_MAKERRANK : PacketBase
    {
        private const int RANK_SIZE = 10;
        public short Type { get; set; }
        public int[] GID { get; set; }
        public string[] CharacterName { get; set; }
        public int[] Point { get; set; }

        public IZ_NOTIFY_MAKERRANK()
        {
            Command = (ushort) PACKET_COMMAND.IZ_NOTIFY_MAKERRANK;
        }

        public IZ_NOTIFY_MAKERRANK(byte[] packet)
            : base(packet)
        {
            using (var ms = new MemoryStream(packet))
            {
                using (var br = new BinaryReader(ms))
                {
                    Command = br.ReadUInt16();
                    Type = br.ReadInt16();

                    GID = new int[RANK_SIZE];
                    for (var i = 0; i < RANK_SIZE; i++)
                    {
                        GID[i] = br.ReadInt32();
                    }

                    CharacterName = new string[RANK_SIZE];
                    for (var i = 0; i < RANK_SIZE; i++)
                    {
                        CharacterName[i] = br.ReadCString(24);
                    }

                    Point = new int[RANK_SIZE];
                    for (var i = 0; i < RANK_SIZE; i++)
                    {
                        Point[i] = br.ReadInt32();
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
            bw.Write(Type);

            for (var i = 0; i < RANK_SIZE; i++)
            {
                bw.Write(GID[i]);
            }

            for (var i = 0; i < RANK_SIZE; i++)
            {
                bw.WriteCString(CharacterName[i], 24);
            }

            for (var i = 0; i < RANK_SIZE; i++)
            {
                bw.Write(Point[i]);
            }
        }
    }
}