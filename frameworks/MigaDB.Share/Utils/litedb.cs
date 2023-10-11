using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public static class litedb
    {
        public const string ID = "_id";
        
        public static bool HasID<T>(this ILiteCollection<T> db, string id)
        {
            return db.Exists(Query.EQ(ID, id));
        }
        
        
        public static bool HasName<T>(this ILiteCollection<T> db, string name)
        {
            return db.Exists(Query.EQ("name", name));
        }
    }
}