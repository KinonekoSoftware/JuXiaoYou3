using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;


namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class DocumentService : DirectoryService
    {
        private const string SubFolders             = "Documents";
        private const string CacheFolders           = "Caches";
        private const string FieldName              = "Name";

        public DocumentService() : base(SubFolders)
        {
        }

        public void Add(Document document)
        {
            if (document is null)
            {
                return;
            }
        }

        public void Update(Document document, bool overwritten = true)
        {
            if (document is null)
            {
                return;
            }
        }

        public void Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
        }
        
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {

           // Database = context.Database.GetCollection<Document>(Constants.cn_document);

            //
            //
            base.OnRepositoryOpening(context, property);
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            //
            //
            base.OnRepositoryClosing(context);
        }

    }
}