using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aegis.CrossCutting.GlobalDataClasses
{
    public static class StringExtensions
    {
        private static readonly byte[] CryptKey = { 0x79, 0x08, 0x49, 0x64, 0xAE, 0x8F, 0xB5, 0x0E };

        public static string Decrypt(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var toDecryptArray = StringToByteArray(input);
            var tdes = new DESCryptoServiceProvider { Key = CryptKey, Mode = CipherMode.ECB, Padding = PaddingMode.Zeros };
            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            tdes.Clear();
            return Encoding.GetEncoding("ISO-8859-1").GetString(resultArray).CutZeroByte();
        }

        public static string CutZeroByte(this string value)
        {
            return value.IndexOf('\0') > -1 ? value.Substring(0, value.IndexOf('\0')) : value;
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }
    }
}
