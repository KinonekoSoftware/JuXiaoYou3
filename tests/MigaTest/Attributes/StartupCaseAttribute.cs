namespace Acorisoft.FutureGL.MigaTest.Attributes
{
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StartupCaseAttribute : LifetimeAttribute
    {
        public StartupCaseAttribute(TestCase testCase) : base(testCase)
        {
        }

        public StartupCaseAttribute(Type testCase): base(testCase)
        {
        }
    }
}