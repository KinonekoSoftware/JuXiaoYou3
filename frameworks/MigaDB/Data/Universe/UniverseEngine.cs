namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public class UniverseEngine : KnowledgeEngine
    {
        public override Knowledge GetKnowledge(string id)
        {
            return null;
        }

        protected override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
        }

        protected override void OnDatabaseClosingOverride()
        {
        }
    }
}