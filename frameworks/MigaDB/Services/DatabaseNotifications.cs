using MediatR;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    /// <summary>
    /// <see cref="DatabaseNotification"/> 表示一个数据库通知。
    /// </summary>
    public abstract class DatabaseNotification : INotification
    {
        /// <summary>
        /// 获取当前的数据库会话。
        /// </summary>
        public DatabaseSession Session { get; init; }
        
        /// <summary>
        /// 获取当前的数据库同步工具。
        /// </summary>
        public IDatabaseSynchronizer Synchronizer { get; init; }
    }
    
    /// <summary>
    /// <see cref="DatabaseOpenNotification"/> 表示一个数据库打开通知。
    /// </summary>
    public class DatabaseOpenNotification : DatabaseNotification
    {
        
    }
    
    /// <summary>
    /// <see cref="DatabaseCloseNotification"/> 表示一个数据库关闭通知。
    /// </summary>  
    public class DatabaseCloseNotification : DatabaseNotification
    {
        public static readonly DatabaseCloseNotification Instance = new DatabaseCloseNotification();
    }
    
    
    /// <summary>
    /// <see cref="DatabaseUpdateNotification"/> 表示一个数据库更新通知。
    /// </summary>
    public class DatabaseUpdateNotification : DatabaseNotification
    {
        
    }
}