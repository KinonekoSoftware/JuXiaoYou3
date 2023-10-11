using System.IO;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors.Utils
{
    public static class RichTextBoxUtilities
    {
        public static void SetText(this RichTextBox rtf, string value)
        {
            if (rtf is null ||
                string.IsNullOrEmpty(value))
            {
                return;
            }

            var document = rtf.Document;
            var ms       = new MemoryStream(Encoding.UTF8.GetBytes(value));
            var range    = new TextRange(document.ContentStart, document.ContentEnd);
            range.Load(ms, DataFormats.Rtf);
        }
        
        public static string GetText(this RichTextBox rtf)
        {
            if (rtf is null)
            {
                return string.Empty;
            }

            var document = rtf.Document;
            var ms       = new MemoryStream(4096);
            var range    = new TextRange(document.ContentStart, document.ContentEnd);
            range.Save(ms, DataFormats.Rtf);
            return Encoding.UTF8.GetString(ms.GetBuffer());
        }
    }
}