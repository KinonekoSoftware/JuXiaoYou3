namespace Acorisoft.FutureGL.MigaDB.Data.Relatives
{
    public class AttachableTypeRelative
    {
        [BsonId]
        public string DocumentID { get; init; }
        
        public int Type { get; init; }
    }
}