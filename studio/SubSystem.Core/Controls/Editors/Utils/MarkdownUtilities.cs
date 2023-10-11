using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public static class MarkdownUtilities
    {
        public static string GetString(this HeadingBlock block)
        {
            var sb = Pool.GetStringBuilder();
            
            block.Inline
                 .OfType<LiteralInline>()
                 .ForEach(x => sb.Append(x.ToString()));
            var s = sb.ToString();
            Pool.ReturnStringBuilder(sb);
            return s;
        }

    }
}