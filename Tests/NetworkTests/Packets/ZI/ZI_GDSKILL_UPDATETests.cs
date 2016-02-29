using Aegis.CrossCutting.Network.Packets.ZI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Tests.NetworkTests.Packets.ZI
{
    [TestClass()]
    public class ZI_GDSKILL_UPDATETests
    {
        [TestMethod()]
        public void ZI_GDSKILL_UPDATETest()
        {
            var data = "85 29 48 00 0C 00 00 00 00 00 00 00 10 27 01 00 11 27 01 00 12 27 01 00 13 27 01 00 14 27 05 00 15 27 00 00 16 27 04 00 17 27 03 00 18 27 03 00 19 27 03 00 1A 27 01 00 1B 27 01 00 1C 27 00 00 1D 27 01 00 1E 27 00 00".ToByteArray();
            var packet = new ZI_GDSKILL_UPDATE(data);
        }
    }
}