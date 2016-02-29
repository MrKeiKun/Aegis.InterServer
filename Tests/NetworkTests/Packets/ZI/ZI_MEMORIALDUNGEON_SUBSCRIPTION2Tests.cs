using Aegis.CrossCutting.Network.Packets.ZI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Tests.NetworkTests.Packets.ZI
{
    [TestClass()]
    public class ZI_MEMORIALDUNGEON_SUBSCRIPTION2Tests
    {
        [TestMethod()]
        public void ZI_MEMORIALDUNGEON_SUBSCRIPTION2Test()
        {
            var data = "79 2A 5F 00 75 6E 6B 6E 6F 77 6E 00 94 E3 92 77 01 29 A6 3B 00 00 00 00 AC 01 9C 00 38 F3 DE 04 A0 B0 4D 00 5C F3 DE 04 B0 F9 BF 0A ED F5 52 00 B0 F9 BF 0A 54 F3 DE 04 49 1E 53 00 F3 86 01 00 00 E7 03 00 00 F3 86 01 00 71 89 01 00 57 61 76 65 20 4D 6F 64 65 20 2D 20 46 6F 72 65 73 74".ToByteArray();
            var packet = new ZI_MEMORIALDUNGEON_SUBSCRIPTION2(data);
        }
    }
}