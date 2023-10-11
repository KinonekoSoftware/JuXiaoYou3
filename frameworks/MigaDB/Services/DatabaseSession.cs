using NLog;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public class DatabaseSession
    {
        /// <summary>
        /// 是否为调试模式。
        /// </summary>
        public bool DebugMode { get; init; }
        
        /// <summary>
        /// 数据库
        /// </summary>
        public IDatabase Database { get; init; }
        
        /// <summary>
        /// 根目录
        /// </summary>
        public string RootDirectory { get; init; }

        public ILogger Logger { get; init; }
    }
}