namespace Acorisoft.FutureGL.MigaDB.UnitTests
{
    public class DatabaseAdapter
    {
        public DatabaseAdapter()
        {
            Directory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public void Stop()
        {
            Database.Dispose();
        }

        public void Open()
        {
            DbStream  = new MemoryStream();
            LogStream = new MemoryStream();
            Kernel    = new LiteDatabase(DbStream, BsonMapper.Global, LogStream);
            Database  = new Database(Kernel);
        }

        public void Reopen()
        {
            Stop();
            
            DbStream.Seek(0, SeekOrigin.Begin);
            LogStream.Seek(0, SeekOrigin.Begin);
            Kernel   = new LiteDatabase(DbStream, BsonMapper.Global, LogStream);
            Database = new Database(Kernel);
        }
        
        public Database Database { get; private set; }
        public LiteDatabase Kernel { get; private set; }
        public MemoryStream DbStream{ get; private set; }
        public MemoryStream LogStream{ get; private set; }
        public string Directory { get; }
    }
}