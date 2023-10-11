namespace Acorisoft.FutureGL.MigaTest.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConstructorCaseAttribute : LifetimeAttribute
    {
        public ConstructorCaseAttribute(TestCase testCase) : base(testCase)
        {
        }

        public ConstructorCaseAttribute(Type testCase): base(testCase)
        {
        }
    }

    public abstract class LifetimeAttribute : Attribute
    {
        protected LifetimeAttribute(TestCase testCase)
        {
            UseSpecifiedTestCase = false;
            BuiltinTestCase      = testCase;
        }

        protected LifetimeAttribute(Type testCase)
        {
            UseSpecifiedTestCase = true;
            SpecifiedTestCase    = testCase;
        }

        public string InvokeMethodBeforeUnitTest { get; set; }
        public string InvokeMethodAfterUnitTest { get; set; }
        public bool UseSpecifiedTestCase { get; }
        public TestCase BuiltinTestCase { get; }
        public Type SpecifiedTestCase { get; }
    }
}