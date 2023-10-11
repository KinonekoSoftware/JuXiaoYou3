namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
{
    public interface IDocumentMetadataCollection : ICollection<Metadata>
    {
        Metadata this[int index] { get; set; }
        Metadata this[string name] { get; set; }
    }
}