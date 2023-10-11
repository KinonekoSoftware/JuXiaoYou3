using System.Data;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class NamePoolService : StorageService, IRefreshSupport
    {
        public NamePoolService()
        {
            Mapping    = new Dictionary<string, Moniker>(13);
            IsLazyMode = true;
        }
        
        public void Refresh()
        {
           Mapping.Clear();
        } 

        public void Add(Moniker moniker)
        {
            if (moniker is null || string.IsNullOrEmpty(moniker.Name))
            {
                return;
            }

            if (Mapping.TryAdd(moniker.Name, moniker))
            {
            }
        }
        
        public void Remove(Moniker moniker)
        {
            if (moniker is null || string.IsNullOrEmpty(moniker.Name))
            {
                return;
            }

            Mapping.Remove(moniker.Name);
            
        }
        
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            // Database = context.Database.GetCollection<Moniker>(Constants.cn_moniker);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Mapping.Clear();
        }
        
        public Dictionary<string, Moniker> Mapping { get; }
    }
}