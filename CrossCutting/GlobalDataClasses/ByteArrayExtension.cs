using System;
using System.Collections.Generic;
using System.Text;

namespace Aegis.CrossCutting.GlobalDataClasses
{
    public static class ByteArrayExtension
    {
        public static IEnumerable<T> Enumerate<T>(this T[] array, int start, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            for (int i = 0; i < count; i++)
                yield return array[start + i];
        }

        public static byte[] Append(this byte[] a, byte[] b)
        {
            var result = new byte[a.Length + b.Length];

            a.CopyTo(result, 0);
            b.CopyTo(result, a.Length);

            return result;
        }

        private static void AppendLine(StringBuilder result, string str)
        {
            result.AppendLine(str);
        }

        private static char ToHexChar(byte b)
        {
            return (b >= 0x20 && b < 0x80) ? (char) b : '.';
        }

        public static string Hexdump(this byte[] buffer)
        {
            var b = new StringBuilder();
            b.AppendLine();
            AppendLine(b, "[0x" + buffer.Length.ToString("X8") + " (" + buffer.Length + ")] {");

            int length = Math.Min(buffer.Length, 0xFFFF);
            for (int i = 0; i < length; i += 16)
            {
                b.AppendFormat("  {0:X4}  ", i);
                for (int n = 0; n < 8; n++)
                {
                    int o = i + n;
                    if (o < length)
                        b.AppendFormat("{0:X2} ", buffer[o]);
                    else
                        b.Append("   ");
                }
                b.Append(" ");
                for (int n = 0; n < 8; n++)
                {
                    int o = i + n + 8;
                    if (o < length)
                        b.AppendFormat("{0:X2} ", buffer[o]);
                    else
                        b.Append("   ");
                }
                b.Append(" ");

                for (int n = 0; n < 8; n++)
                {
                    int o = i + n;
                    if (o < length)
                        b.AppendFormat("{0}", ToHexChar(buffer[o]));
                    else
                        b.Append(" ");
                }
                b.Append(" ");
                for (int n = 0; n < 8; n++)
                {
                    int o = i + n + 8;
                    if (o < length)
                        b.AppendFormat("{0}", ToHexChar(buffer[o]));
                    else
                        b.Append(" ");
                }
                b.AppendLine();
            }
            AppendLine(b, "}");
            return b.ToString();
        }
    }
}