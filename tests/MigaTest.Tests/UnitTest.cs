using Acorisoft.FutureGL.MigaTest;

namespace MigaTest.Tests
{
    [TestClass]
    public class UnitTester
    {
        [TestMethod]
        public void Initialize()
        {
            Tester.Run<UnitTester>();
        }
    }
}