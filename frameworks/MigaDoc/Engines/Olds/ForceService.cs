using Acorisoft.Miga.Doc.Entities;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ForceService : StorageService
    {
        private const string CollectionName = "forces";
        
        public ForceService()
        {
            IsLazyMode = true;
        }

        public void Add(Force entity)
        {
            if (entity is null)
            {
                return;
            }
        }

        public void Remove(Force entity)
        {
            if (entity is null)
            {
                return;
            }
        }
        
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
           // Database = context.Database.GetCollection<Force>(CollectionName);
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
        }
        
    }
}