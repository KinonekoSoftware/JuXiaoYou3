namespace Acorisoft.FutureGL.MigaTest.Attributes
{
    
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StartCaseAttribute : LifetimeAttribute
    {
        public StartCaseAttribute(TestCase testCase) : base(testCase)
        {
        }

        public StartCaseAttribute(Type testCase): base(testCase)
        {
        }
    }

}