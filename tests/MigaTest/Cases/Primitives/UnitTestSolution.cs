using System.Diagnostics;
using Acorisoft.FutureGL.MigaTest.Attributes;
using Acorisoft.FutureGL.MigaTest.Reflections;
using Acorisoft.FutureGL.MigaTest.Utils;

namespace Acorisoft.FutureGL.MigaTest.Cases.Primitives
{
    public class UnitTestSolution : IUnitTestSolution
    {
        public UnitTestSolution(List<IUnitTestTarget> targets) => Targets = targets.AsReadOnly();
        
        public void Run()
        {
            Trace.WriteLine("构建完闭，开始测试...");
            
            foreach (var unitTestTarget in Targets)
            {
                if (unitTestTarget is not UnitTestTarget target)
                {
                    continue;
                }
                
                //
                // 测试
                AssertViewModel(target);
            }
        }

        private void AssertViewModel(UnitTestTarget target)
        {
            Trace.WriteLine($"正在执行ViewModel单元测试，当前模块:{target.AssemblyName}");
            
            foreach (var vm in target.Case_ViewModel)
            {
                PrepareLifetimeProcedure<ConstructorCaseAttribute>(vm);
                PrepareLifetimeProcedure<StartupCaseAttribute>(vm);
                PrepareLifetimeProcedure<StartCaseAttribute>(vm);
                PrepareLifetimeProcedure<RunningCaseAttribute>(vm);
                PrepareLifetimeProcedure<StopCaseAttribute>(vm);
            }
        }

        private void PrepareLifetimeProcedure<T>(ClassContext target)
        {
            target.Initialize();
            
            foreach (var field in target.Fields)
            {
                if (!field.Information
                         .IsDefined(typeof(T), true))
                {
                    continue;
                }
                
                
                var b = FieldUtil.CheckThisField(field, target.Instance);
                Assert_True(b, target);
            }
            
        }

        private void Assert_True(bool r, ClassContext target)
        {
            
        }

        public IReadOnlyList<IUnitTestTarget> Targets { get; }
    }
}