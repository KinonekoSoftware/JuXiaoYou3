using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Acorisoft.FutureGL.MigaStudio.Editors.Models
{
    public class ConceptCompletionData : ICompletionData
    {
        // TODO: https://www.cnblogs.com/nankezhishi/archive/2008/11/22/1338959.html
        // TODO: https://www.cnblogs.com/Soar1991/p/15834391.html
        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document
                    .Replace(completionSegment, Text);
        }

        public ImageSource Image { get;init; }
        public string Text { get;init; }
        public object Content { get;init; }
        public object Description { get;init; }
        public double Priority { get; init; }
    }
}