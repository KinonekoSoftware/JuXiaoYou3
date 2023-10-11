using ComposeIndex = Acorisoft.Miga.Doc.Documents.ComposeIndex;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ComposeIndexService : StorageService, IRefreshSupport
    {
        public ComposeIndexService()
        {
        }

        public void Add(ComposeIndex index)
        {
            if (index is null)
            {
                return;
            }

        }


        public void Remove(ComposeIndex index)
        {          
        }

        public void Refresh()
        {
        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            // Database = context.Database.GetCollection<ComposeIndex>(Constants.cn_index_compose);
            // Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
        }
    }
}