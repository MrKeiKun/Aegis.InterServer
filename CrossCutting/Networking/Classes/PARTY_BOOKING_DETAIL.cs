using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegis.CrossCutting.Network.Classes
{
    public class PARTY_BOOKING_DETAIL
    {
        public short Level { get; set; }
        public short MapID { get; set; }
        public short[] Job { get; set; }

        public PARTY_BOOKING_DETAIL(BinaryReader br)
        {
            Level = br.ReadInt16();
            MapID = br.ReadInt16();
            Job = new short[6];
            for (var i = 0; i < Job.Length; i++)
            {
                Job[i] = br.ReadInt16();
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Level);
            bw.Write(MapID);
            for (var i = 0; i < Job.Length; i++)
            {
                bw.Write(Job[i]);
            }
        }
    }
}
