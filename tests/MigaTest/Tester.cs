using System.Diagnostics;
using System.Reflection;
using Acorisoft.FutureGL.MigaTest.Attributes;
using Acorisoft.FutureGL.MigaTest.Cases;
using Acorisoft.FutureGL.MigaTest.Cases.Primitives;
using Acorisoft.FutureGL.MigaTest.Reflections;

namespace Acorisoft.FutureGL.MigaTest
{
    public static class Tester
    {
        public static void Run<T>()
        {
            Trace.Listeners
                 .Add(new TextWriterTraceListener(Console.Out));

            var references = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                      .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
            
            var assemblies = references.Where(x => x.IsDefined(typeof(UnitTestTargetAttribute), false))
                                       .ToArray();

            if (assemblies is null ||
                assemblies.Length == 0)
            {
                Trace.WriteLine("没有发现需要进行单元测试的目标");
                return;
            }

            //
            // 创建解决方案
            Trace.WriteLine("正在构建单元测试用例...");
            var solution = Build(assemblies);
            

            //
            // 开始测试
            solution.Run();
        }

        private static IUnitTestSolution Build(IEnumerable<Assembly> assemblies)
        {
            //
            // 创建解决方案
            var solution = assemblies.Select(BuildTarget)
                                     .Where(target => target is not null)
                                     .ToList();

            //
            // 开始测试
            Trace.WriteLine($"构建单元测试用例完成，总计目标：{solution.Count}。");
            return new UnitTestSolution(solution);
        }

        private static IUnitTestTarget BuildTarget(Assembly assembly)
        {
            if (assembly is null)
            {
                return null;
            }

            var unknownTypes = assembly.GetTypes();
            var target = new UnitTestTarget(assembly.Modules
                                                    .FirstOrDefault()
                                                    ?.Name);
            var sampler      = 0;
                
            // build ViewModelTarget

            foreach (var unknownType in unknownTypes)
            {
                if (unknownType.IsAbstract || 
                    unknownType.IsInterface ||
                    unknownType.IsEnum)
                {
                    continue;
                }

                if (!unknownType.IsClass)
                {
                    continue;
                }
                
                if (unknownType.Name
                               .EndsWith("ViewModel"))
                {
                    if (sampler++ % 5 == 0)
                    {
                        Trace.WriteLine($"正在添加测试单元，目标：{unknownType.Name}");
                    }

                    target.Case_ViewModel.Add(ClassContext.FromType(unknownType));
                    continue;
                }
                
                
                
                if (unknownType.Name
                               .EndsWith("Model"))
                {
                    // Trace.WriteLine($"正在添加测试单元，目标：{unknownType.Name}！");
                    // target.Case_Model.Add(unknownType);
                    continue;
                }
                
                
                // if (unknownType.Name
                //                .EndsWith("ViewModel"))
                // {
                //     target.Case_Control.Add(unknownType);
                //     continue;
                // }
                
                continue;
                Trace.WriteLine($"正在添加测试单元，目标：{unknownType.Name}！");
            }

            return target;
        }
    }
}