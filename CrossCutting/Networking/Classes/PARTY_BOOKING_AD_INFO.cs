using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.CrossCutting.GlobalDataClasses;

namespace Aegis.CrossCutting.Network.Classes
{
    public class PARTY_BOOKING_AD_INFO
    {
        public int Index { get; set; }
        public string CharName { get; set; }
        public int ExpireTime { get; set; }
        public PARTY_BOOKING_DETAIL Detail { get; set; }

        public PARTY_BOOKING_AD_INFO(BinaryReader br)
        {
            Index = br.ReadInt32();
            CharName = br.ReadCString(24);
            ExpireTime = br.ReadInt32();
            Detail = new PARTY_BOOKING_DETAIL(br);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Index);
            bw.WriteCString(CharName, 24);
            bw.Write(ExpireTime);
            Detail.Write(bw);
        }
    }
}
