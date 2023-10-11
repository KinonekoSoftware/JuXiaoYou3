using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(CharacterDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(ItemDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(SkillDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(OtherDocumentViewModel))]
    public partial class DocumentEditorPage
    {
        public DocumentEditorPage()
        {
            InitializeComponent();
        }
    }
}