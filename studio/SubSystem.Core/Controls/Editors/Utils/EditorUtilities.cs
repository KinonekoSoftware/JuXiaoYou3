using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public static class EditorUtilities
    {
        public static IComposeEditor Register(this IComposeEditor editor, RichTextBox control)
        {
            if (editor.InternalWorkspaceCollection
                      .FirstOrDefault(x => x is RichTextFileWorkspace) is not RichTextFileWorkspace w)
            {
                w = new RichTextFileWorkspace
                {
                    Control = control
                };
                
                editor.InternalWorkspaceCollection
                      .Add(w);
            }

            else
            {
                w.Control = control;
            }
            
            return editor;
        }
        
        public static IComposeEditor Register(this IComposeEditor editor,TextEditor control)
        {
            if (editor.InternalWorkspaceCollection
                      .FirstOrDefault(x => x is MarkdownWorkspace) is not MarkdownWorkspace w)
            {
                w = new MarkdownWorkspace
                {
                    Control = control
                };
                
                editor.InternalWorkspaceCollection
                      .Add(w);
            }

            else
            {
                w.Control = control;
            }
            return editor;
        }

        public static IWorkspace CreateFromMarkdownPart(PartOfMarkdown md)
        {
            return new MarkdownWorkspace()
            {
                Part = md
            };
        }
    }
}