namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public enum EngineFailedReason
    {
        /// <summary>
        /// 用户取消
        /// </summary>
        Cancelled,
        
        /// <summary>
        /// 没有改变
        /// </summary>
        NoChanged,
        
        /// <summary>
        /// 缺失参数
        /// </summary>
        MissingParameter,
        
        /// <summary>
        /// Id为空
        /// </summary>
        MissingId,
        
        /// <summary>
        /// 重复
        /// </summary>
        Duplicated,
        
        /// <summary>
        /// 数据一致性被破坏
        /// </summary>
        ConsistencyBroken,
        
        /// <summary>
        /// 参数或者执行中遇到的数据错误
        /// </summary>
        InputDataError,
        
        /// <summary>
        /// 参数为空
        /// </summary>
        ParameterEmptyOrNull,
        
        /// <summary>
        /// 参数依赖为空，例如 DocumentCache里面，虽然DocumentCache不为空，但是Owner为空
        /// </summary>
        ParameterDependencyEmptyOrNull,
        
        /// <summary>
        /// 参数或者执行中遇到的文件无访问权限
        /// </summary>
        InputSourceUnauthorizedAccess,
        
        /// <summary>
        /// 参数或者执行中遇到的文件不存在
        /// </summary>
        InputSourceNotExists,
        
        /// <summary>
        /// 参数或者执行中遇到的文件占用
        /// </summary>
        InputSourceOccupied,
        
        /// <summary>
        /// 未知的错误
        /// </summary>
        Unknown,
    }
    
    public class EngineResult
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public EngineFailedReason Reason { get; init; }

        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static EngineResult Failed(EngineFailedReason reason) => new EngineResult
        {
            IsFinished = false,
            Reason     = reason
        };

        /// <summary>
        /// 表示一个成功的结果。
        /// </summary>
        public static readonly EngineResult Successful = new EngineResult { IsFinished = true };
    }

    public class EngineResult<T> : EngineResult
    {
        public static EngineResult<T> Finished(EngineFailedReason reason)
        {
            return new EngineResult<T>
            {
                IsFinished = false,
                Reason     = reason
            };
        }
        
        public static EngineResult<T> Finished(T result)
        {
            return new EngineResult<T>
            {
                IsFinished = true,
                Result     = result
            };
        }
        
        public T Result { get; init; }
    }
}