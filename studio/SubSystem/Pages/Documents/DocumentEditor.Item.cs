
namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class ItemDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<HeaderedSubView> collection)
        {
            AddBasicView<ItemBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
        }
        
        
        protected override void OnCreateDocument(Document document)
        {
            AddDataPartToDocument(CreateAlbum());
            AddDataPartToDocument(CreateStickyNote());
        }

    }
}