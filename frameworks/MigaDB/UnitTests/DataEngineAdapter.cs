namespace Acorisoft.FutureGL.MigaDB.UnitTests
{
    public class DataEngineAdapter
    {
        public DataEngineAdapter(DatabaseAdapter adapter)
        {
            Adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }
        
        /// <summary>
        /// 获取当前测试的数据库适配器。
        /// </summary>
        public DatabaseAdapter Adapter { get;}
    }

    public class DataEngineAdapter<TEngine> : DataEngineAdapter where TEngine : DataEngine
    {
        public DataEngineAdapter(TEngine engine) : base(new DatabaseAdapter())
        {
            //
            // 创建引擎。
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        /// <summary>
        /// 开始托管
        /// </summary>
        public void Start()
        {
            Adapter.Open();
            Engine.DatabaseOpeningForTesting(Adapter);
        }

        public void Restart()
        {
            Stop();
            Adapter.Reopen();
            Engine.DatabaseOpeningForTesting(Adapter);
        }

        public void Stop()
        {
            Adapter.Stop();
            Engine.DatabaseClosingForTesting();
        }
        
        /// <summary>
        /// 获取当前测试的引擎。
        /// </summary>
        public TEngine Engine { get; }
    }
}