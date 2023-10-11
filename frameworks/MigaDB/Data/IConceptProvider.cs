namespace Acorisoft.FutureGL.MigaDB.Data.Concepts
{
    public interface IConceptProvider
    {
        Knowledge GetKnowledge(string id);
        IEnumerable<Concept> GetConcepts();
        IEnumerable<Concept> GetConcepts(Predicate<Concept> predicate);
        Concept GetConcept(string id);
    }
}