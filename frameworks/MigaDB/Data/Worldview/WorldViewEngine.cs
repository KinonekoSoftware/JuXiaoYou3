using Acorisoft.FutureGL.MigaDB.Documents;
using static Acorisoft.FutureGL.MigaDB.Constants;

namespace Acorisoft.FutureGL.MigaDB.Data.Worldview
{
    public class WorldViewEngine : KnowledgeEngine
    {
        public override Knowledge GetKnowledge(string id)
        {
            throw new NotImplementedException();
        }

        protected override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            DocumentDB      = database.GetCollection<Document>(Name_Document);
            DocumentCacheDB = database.GetCollection<DocumentCache>(Name_Cache_Document);
            ObjectDB        = database.GetCollection<WorldViewObject>(Name_Object);
        }

        protected override void OnDatabaseClosingOverride()
        {
            DocumentDB      = null;
            DocumentCacheDB = null;
            ObjectDB        = null;
        }
        
        public ILiteCollection<WorldViewObject> ObjectDB { get; private set; }
        public ILiteCollection<Document> DocumentDB { get; private set; }
        public ILiteCollection<DocumentCache> DocumentCacheDB { get; private set; }
    }
}