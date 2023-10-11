using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Interfaces
{
    public interface IObjectCollection
    {
        ILiteCollection<BsonDocument> Props { get; }
    }
}