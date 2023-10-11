using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MigaStudio.TestGen
{
    
    [Generator]
    public class UnitTestGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var visitor     = new ClassSyntaxVisitor();

            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                visitor.Visit(root);
            }
            
            visitor.Dump(@"D:\Code\UnitTest");
        }
    }
}