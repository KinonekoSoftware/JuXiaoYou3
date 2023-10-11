using Acorisoft.FutureGL.MigaTest.Reflections;

namespace Acorisoft.FutureGL.MigaTest.Cases.Primitives
{
    public class UnitTestTarget : IUnitTestTarget
    {
        public UnitTestTarget(string name)
        {
            AssemblyName   = name;
            Case_ViewModel = new List<ClassContext>(64);
            Case_Model     = new List<ClassContext>(32);
            Case_Control   = new List<ClassContext>(32);
            Case_Other     = new List<ClassContext>(32);
        }

        public string AssemblyName { get; }
        public IList<ClassContext> Case_ViewModel { get; }
        public IList<ClassContext> Case_Model { get; }
        public IList<ClassContext> Case_Control { get; }
        public IList<ClassContext> Case_Other { get; }
    }
}