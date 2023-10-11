using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Acorisoft.MigaGen.App
{
    [Generator]
    public class UIGen : ISourceGenerator
    {
        private const string Template = @"  
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
{0}

namespace N506885bf54c6413d99e0b54a5f0f5475
{{
    public partial class NClass: IViewModelRegister
    {{
        public void Register(IViewInstaller collection)
        {{
            AddViewModels(collection);
            AddProperty(collection);
        }}
                        
        public void AddViewModels(IViewInstaller collection)
        {{
            {1}
        }}

        public void AddProperty(IViewInstaller collection)
        {{
            {2}
        }}
    }}
}}";
        
        public void Initialize(GeneratorInitializationContext context)
        {
            Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var visitor = new PageTokenDetector();
            var nsVisitor = new NamespaceDetector();
            var textBuilder = new StringBuilder();

            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                nsVisitor.Visit(root);
                visitor.Visit(root);
            }
            
            var nsStub = nsVisitor.GetNamespaceBlock();
            textBuilder.Clear();

            foreach (var ns in visitor.BindingInfo)
            {
                if (string.IsNullOrEmpty(ns))
                {
                    continue;
                }

                textBuilder.Append(ns);
            }

            var pageStub = textBuilder.ToString();
            textBuilder.Clear();

            foreach (var bi in visitor.MetadataInfo)
            {
                if (string.IsNullOrEmpty(bi))
                {
                    continue;
                }

                textBuilder.Append(bi);
            }

            var propStub = textBuilder.ToString();
            textBuilder.Clear();
            
            
            var code = string.Format(Template, nsStub, pageStub, propStub);
#if DEBUG
            //System.IO.File.WriteAllText(@"E:\gen.cs", code);
#endif
            context.AddSource("N506885bf54c6413d99e0b54a5f0f5475.cs", SourceText.From(code, Encoding.UTF8));
        }
    }

    #region Namespace Detector

    internal class NamespaceDetector : CSharpSyntaxWalker
    {
        public readonly HashSet<string> _pool;

        public NamespaceDetector()
        {
            _pool = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public string GetNamespaceBlock()
        {
            var sb = new StringBuilder();
            foreach (var ns in _pool)
            {
                sb.Append(ns);
            }

            return sb.ToString();
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var ns = GetNamespace(node);
            _pool.Add($"using {ns};\n");
        }

        public static string GetNamespace(NamespaceDeclarationSyntax node)
        {
            return node?.Name
                .ToFullString()
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();
        }

        public IEnumerable<string> Namespaces => _pool;
    }

    #endregion

    public class Page
    {
        public string View { get; set; }
        public string ViewModel { get; set; }
    }

    public class Property : Attribute
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string UseResourceKey { get; set; }
    }

    #region PageToken Detector

    internal class PageTokenDetector : CSharpSyntaxWalker
    {
        private const string PropertyDefaultString = "\"\"";
        private readonly List<string> _tokenInfo;
        private readonly List<string> _metadata;

        private const string add_view = "collection.Install(new BindingInfo(typeof({0}),typeof({1})));\n";

        private const string add_property =
            "collection.Add(new BindingInfo{{ Id = {0}, Group = {1}, Name = {2}, UseResourceKey = {3} }});\n";

        public PageTokenDetector()
        {
            _tokenInfo = new List<string>(128);
            _metadata = new List<string>(128);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Property property = null;
            
            foreach (var attr in node.AttributeLists)
            {
                var pageInfo = attr.Attributes.FirstOrDefault(x => x.Name.ToFullString() == "Connected");
                var titleInfo = attr.Attributes.FirstOrDefault(x => x.Name.ToFullString() == "Title");
                var guidInfo = attr.Attributes.FirstOrDefault(x => x.Name.ToFullString() == "Guid");

                if (!(pageInfo?.ArgumentList is null))
                {
                    GetPageTokenAttribute(pageInfo.ArgumentList);
                }

                if (!(titleInfo?.ArgumentList is null))
                {
                    GetPageTitleAttribute(titleInfo.ArgumentList, out property);
                }

                if (!(guidInfo?.ArgumentList is null))
                {
                    GetGuidAttribute(property, guidInfo.ArgumentList);
                }

            }

            if (property is null ||
                string.IsNullOrEmpty(property.Id))
            {
                return;
            }
            
            _metadata.Add(
                string.Format(add_property,
                    IsDefault(property.Id, PropertyDefaultString),
                    property.Group,
                    property.Name,
                    property.UseResourceKey));
        }

        private void GetPageTitleAttribute(AttributeArgumentListSyntax info, out Property property)
        {
            property = new Property();

            foreach (var arg in info.Arguments)
            {
                var name = arg.NameEquals?.ToFullString().Replace("=", "").Trim();
                var value = arg.Expression.ToFullString();

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                switch (name)
                {
                    case "Group":
                        property.Group = value;
                        break;
                    case "UseResourceKey":
                        property.UseResourceKey = value;
                        break;
                    case "Name":
                        property.Name = value;
                        break;
                }
            }

            property.Name = IsDefault(property.Name, PropertyDefaultString);
            property.Group = IsDefault(property.Group, PropertyDefaultString);
            property.Id = IsDefault(property.Id, PropertyDefaultString);
            property.UseResourceKey = IsDefault(property.UseResourceKey, "false");
        }

        private void GetGuidAttribute(Property property, AttributeArgumentListSyntax info)
        {
            if (property is null) return;
            foreach (var arg in info.Arguments)
            {
                var value = arg.Expression.ToFullString();
                
                property.Id = IsDefault(value, PropertyDefaultString);
            }
        }

        private void GetPageTokenAttribute(AttributeArgumentListSyntax info)
        {
            var pInfo = new Page();

            foreach (var arg in info.Arguments)
            {
                var name = arg.NameEquals?.ToFullString().Replace("=", "").Trim();
                var value = arg.Expression.ToFullString();

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                switch (name)
                {
                    case "ViewModel":
                        pInfo.ViewModel = value;
                        break;
                    case "View":
                        pInfo.View = value;
                        break;
                }
            }
            _tokenInfo.Add(string.Format(
                add_view,
                GetTypeOfString(pInfo.View),
                GetTypeOfString(pInfo.ViewModel)));
        }

        private static string IsDefault(string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        private static string GetTypeOfString(string raw)
        {
            return raw.Length <= 8 ? raw : raw.Substring(7, raw.Length - 8);
        }

        public IEnumerable<string> BindingInfo => _tokenInfo;
        public IEnumerable<string> MetadataInfo => _metadata;
    }

    #endregion
}