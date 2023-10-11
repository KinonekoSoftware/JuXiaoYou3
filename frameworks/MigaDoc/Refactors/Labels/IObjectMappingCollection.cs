namespace Acorisoft.Miga.Doc.Labels
{
    public interface IObjectMappingCollection<in T> where T : class, IObjectMapping
    {
        void AddMapping(T mapping);
        IEnumerable<IObjectMapping> GetMappings(IObjectLabel label);

        void RemoveMapping(IObjectLabel label, string targetId);
    }
}