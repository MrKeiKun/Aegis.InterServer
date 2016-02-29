using System.IO;

namespace Aegis.CrossCutting.GlobalDataClasses
{
    public static class BinaryReaderExtension
    {
        public static string ReadCString(this BinaryReader br, int size)
        {
            int i;
            var str = string.Empty;

            for (i = 0; i < size; i++)
            {
                byte b = br.ReadByte();

                if (b == 0)
                    break;

                str += (char) b;
            }

            if (i < size)
                br.ReadBytes(size - i - 1);

            return str;
        }

        public static string ReadCString(this BinaryReader br)
        {
            var str = string.Empty;
            do
            {
                byte b = br.ReadByte();

                if (b == 0)
                    break;

                str += (char) b;
            } while (true);

            return str;
        }
    }
}