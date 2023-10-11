namespace Acorisoft.FutureGL.MigaUtils
{
    public enum AppReason
    {
        /// <summary>
        /// 占用
        /// </summary>
        Occupied,
        
        /// <summary>
        /// 缺少框架服务
        /// </summary>
        MissingService,
        
        /// <summary>
        /// 
        /// </summary>
        FileNotFound,
        
        /// <summary>
        /// 
        /// </summary>
        DirectoryNotFound,
        
        /// <summary>
        /// 用户主动取消
        /// </summary>
        CanceledByUser,
        
        /// <summary>
        /// 应用程序主动取消
        /// </summary>
        CanceledByApp,
        
        /// <summary>
        /// 超时
        /// </summary>
        TimeOut,
    }
    
    /// <summary>
    /// 表示一个操作结果
    /// </summary>
    /// <typeparam name="TValue">原因类型。</typeparam>
    public class Result<TValue>
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public int Reason { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public TValue Value { get; init; }

        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static Result<TValue> Failed(int reason) => new Result<TValue>
        {
            IsFinished = false,
            Reason     = reason
        };
        
        
        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static Result<TValue> Failed<TEnum>(TEnum reason) where TEnum : Enum => new Result<TValue>
        {
            IsFinished = false,
            Reason     = (int)Convert.ChangeType(reason, typeof(TEnum))
        };

        /// <summary>
        /// 表示一个成功的结果。
        /// </summary>
        public static Result<TValue> Successful(TValue value) => new Result<TValue> { Value = value, IsFinished = true };
    }

    /// <summary>
    /// 表示一个操作结果
    /// </summary>
    /// <typeparam name="TValue">结果类型。</typeparam>
    public class IsCompleted<TValue>
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 成功时获取结果。
        /// </summary>
        public TValue Result { get; init; }
        
        
        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <returns>返回一个失败的操作结果</returns>
        public static IsCompleted<TValue> Failed() => new IsCompleted<TValue>
        {
            IsFinished = false
        };

        /// <summary>
        /// 返回一个成功的操作结果以及值。
        /// </summary>
        /// <param name="value">正确操作之后的值</param>
        /// <returns>返回一个成功的操作结果以及值。</returns>
        public static IsCompleted<TValue> Success(TValue value) => new IsCompleted<TValue> { IsFinished = true, Result = value };
    }

    /// <summary>
    /// 表示一个操作结果
    /// </summary>
    /// <typeparam name="TReason">原因类型。</typeparam>
    /// <typeparam name="TValue">结果类型。</typeparam>
    public class Result<TReason, TValue>
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public TReason Reason { get; init; }
        
        /// <summary>
        /// 成功时获取结果。
        /// </summary>
        public TValue Value { get; init; }
        
        
        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static Result<TReason, TValue> Failed(TReason reason) => new Result<TReason, TValue>
        {
            IsFinished = false,
            Reason     = reason
        };

        /// <summary>
        /// 返回一个成功的操作结果以及值。
        /// </summary>
        /// <param name="value">正确操作之后的值</param>
        /// <returns>返回一个成功的操作结果以及值。</returns>
        public static Result<TReason, TValue> Success(TValue value) => new Result<TReason, TValue> { IsFinished = true, Value = value };
    }

    public class Operation<TValue>
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; init; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason { get; init; }
        
        /// <summary>
        /// 成功时获取结果。
        /// </summary>
        public TValue Value { get; init; }
        
        
        /// <summary>
        /// 返回一个失败的操作结果
        /// </summary>
        /// <param name="reason">失败的原因</param>
        /// <returns>返回一个失败的操作结果</returns>
        public static Operation<TValue> Failed(string reason) => new Operation<TValue>
        {
            IsFinished = false,
            Reason     = reason
        };

        /// <summary>
        /// 返回一个成功的操作结果以及值。
        /// </summary>
        /// <param name="value">正确操作之后的值</param>
        /// <returns>返回一个成功的操作结果以及值。</returns>
        public static Operation<TValue> Success(TValue value) => new Operation<TValue>{ IsFinished = true, Value = value };
    }
}