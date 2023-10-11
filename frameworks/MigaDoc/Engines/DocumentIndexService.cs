
namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class DocumentIndexService : StorageService, IRefreshSupport
    {
        public DocumentIndexService()
        {
        }

        public void Add(DocumentIndex index)
        {
            
        }

        public void Remove(DocumentIndex index)
        {
        }
        
        public void Remove(string id)
        {
        }
        
        public void Refresh()
        {
        }


        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            // Database = context.Database.GetCollection<DocumentIndex>(Constants.cn_index);
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
        }
    }
}