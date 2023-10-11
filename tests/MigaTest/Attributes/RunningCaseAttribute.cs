namespace Acorisoft.FutureGL.MigaTest.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RunningCaseAttribute : LifetimeAttribute
    {
        public RunningCaseAttribute(TestCase testCase) : base(testCase)
        {
        }

        public RunningCaseAttribute(Type testCase): base(testCase)
        {
        }
    }
}