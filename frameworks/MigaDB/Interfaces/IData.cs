namespace Acorisoft.FutureGL.MigaDB.Interfaces
{
    public interface IDataPartPackage : IData
    {
        DataPartCollection Parts { get; }
    }

    public interface IMetadataPackage : IDataPartPackage
    {
        MetadataCollection Metas { get; }
    }
}