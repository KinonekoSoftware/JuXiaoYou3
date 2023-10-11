namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public enum DatabaseFailedReason
    {
        /// <summary>
        /// 被占用
        /// </summary>
        Occupied,
        
        /// <summary>
        /// 无权限
        /// </summary>
        UnauthorizedAccess,
        
        /// <summary>
        /// 不存在目录
        /// </summary>
        DirectoryNotExists,
        
        /// <summary>
        /// 不存在数据库
        /// </summary>
        DatabaseNotExists,
        
        /// <summary>
        /// 数据库已经存在
        /// </summary>
        DatabaseAlreadyExists,
        
        /// <summary>
        /// 数据损坏
        /// </summary>
        DatabaseBroken,
        
        /// <summary>
        /// 缺少参数或者为空
        /// </summary>
        MissingParameter,
        
        /// <summary>
        /// 缺少索引
        /// </summary>
        MissingFileName,
        
        /// <summary>
        /// 未知的错误
        /// </summary>
        Unexpected,
        
        /// <summary>
        /// 未打开数据库
        /// </summary>
        DatabaseNotOpen,
    }
    
    public class DatabaseResult
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public DatabaseFailedReason Reason { get; init; }

        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static DatabaseResult Failed(DatabaseFailedReason reason) => new DatabaseResult
        {
            IsFinished = false,
            Reason     = reason
        };

        /// <summary>
        /// 表示一个成功的结果。
        /// </summary>
        public static readonly DatabaseResult Successful = new DatabaseResult { IsFinished = true };
    }
}