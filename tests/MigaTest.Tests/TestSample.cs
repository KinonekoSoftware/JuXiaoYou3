using Acorisoft.FutureGL.MigaTest;
using Acorisoft.FutureGL.MigaTest.Attributes;

namespace MigaTest.Tests
{
    public class TestSample
    {
        [ConstructorCase(TestCase.NotNull)]
        private readonly int? _field;

        [ConstructorCase(TestCase.Boundary)]
        private string _string;
    }
}