namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
{
    public interface IMetadataSource<T>
    {
        T GetValue();
    }
    
    public interface IMetadataTextSource : IMetadataSource<string>
    {
    }
    
    
    public interface IMetadataNumericSource : IMetadataSource<int>
    {
    }
    
    
    public interface IMetadataBooleanSource : IMetadataSource<bool>
    {
    }
}