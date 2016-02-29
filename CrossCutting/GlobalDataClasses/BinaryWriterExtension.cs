using System;
using System.IO;
using System.Text;

namespace Aegis.CrossCutting.GlobalDataClasses
{
    public static class BinaryWriterExtension
    {
        public static void WriteCString(this BinaryWriter bw, string data)
        {
            WriteCString(bw, data, data.Length);
        }

        public static void WriteCString(this BinaryWriter bw, string data, int size)
        {
            if (data == null)
            {
                data = string.Empty;
            }

            if (data.Length > size)
            {
                data = data.Substring(0, size);
            }

            if (data.Length < size)
            {
                data = data.PadRight(size, Convert.ToChar("\0"));
            }

            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(data);
            bw.Write(bytes);
        }
    }
}