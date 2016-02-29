using Aegis.CrossCutting.Network.Packets.ZI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Tests.NetworkTests.Packets.ZI
{
    [TestClass()]
    public class ZI_GUILD_NOTICETests
    {
        [TestMethod()]
        public void ZI_GUILD_NOTICETest()
        {
            var data = "5F 29 0C 00 00 00 44 69 76 69 6E 65 20 50 72 69 64 65 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 53 3A 20 64 69 76 69 6E 65 2D 70 72 69 64 65 2E 6E 65 74 20 61 73 61 64 20 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00".ToByteArray();
            var packet = new ZI_GUILD_NOTICE(data);
        }
    }
}