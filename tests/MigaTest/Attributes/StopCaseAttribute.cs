namespace Acorisoft.FutureGL.MigaTest.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StopCaseAttribute : LifetimeAttribute
    {
        public StopCaseAttribute(TestCase testCase) : base(testCase)
        {
        }

        public StopCaseAttribute(Type testCase): base(testCase)
        {
        }
    }
}