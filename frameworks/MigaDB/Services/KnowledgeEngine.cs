using System.Diagnostics;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Concepts;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public abstract class KnowledgeEngine : DataEngine, IConceptProvider
    {
        public abstract Knowledge GetKnowledge(string id);

        public IEnumerable<Concept> GetConcepts() => ConceptDB.FindAll();

        public IEnumerable<Concept> GetConcepts(Predicate<Concept> predicate) => predicate is null ? null : ConceptDB.Find(x => predicate(x));

        public Concept GetConcept(string id) => ConceptDB.FindById(id);

        public void AddConcept(string id, string name, KnowledgeHandler type)
        {
            if (string.IsNullOrEmpty(id) || 
                string.IsNullOrEmpty(name))
            {
                return;
            }
            
            if (ConceptDB.HasID(id))
            {
                var inside = ConceptDB.FindById(id);
                inside.Name = name;

                if (inside.Handler != type)
                {
                    Debug.WriteLine($"Concept实体冲突，因为类型不一致但是ID一致，所以冗余处理了");
                    ConceptDB.Delete(id);
                    ConceptDB.Insert(new Concept
                    {
                        Id      = id,
                        Name    = name,
                        Handler = type
                    });
                }
                else
                {
                    ConceptDB.Update(inside);
                }
            }
            else
            {
                ConceptDB.Insert(new Concept
                {
                    Id      = id,
                    Name    = name,
                    Handler = type
                });
            }
        }

        public void RemoveConcept(string id)
        {
            ConceptDB.Delete(id);
        }

        protected sealed override void OnDatabaseOpening(DatabaseSession session)
        {
            ConceptDB = session.Database
                               .GetCollection<Concept>(Constants.Name_Concept);
            OnDatabaseOpeningOverride(session);
        }

        protected sealed override void OnDatabaseClosing()
        {
            ConceptDB = null;
            OnDatabaseClosingOverride();
            
        }

        protected abstract void OnDatabaseOpeningOverride(DatabaseSession session);
        protected abstract void OnDatabaseClosingOverride();

        public ILiteCollection<Concept> ConceptDB { get; private set; } 
    }
}