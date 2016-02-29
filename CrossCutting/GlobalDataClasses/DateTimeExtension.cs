using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegis.CrossCutting.GlobalDataClasses
{
    public static class DateTimeExtension
    {
        public static long Timestamp(this DateTime value)
        {
            var epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
    }
}
