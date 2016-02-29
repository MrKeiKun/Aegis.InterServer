using System;
using System.Linq;

namespace Aegis.Tests.NetworkTests
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string text)
        {
            text = text.Replace(" ", "");
            return Enumerable.Range(0, text.Length)
                .Where(x => x%2 == 0)
                .Select(x => Convert.ToByte(text.Substring(x, 2), 16))
                .ToArray();
        }
    }
}