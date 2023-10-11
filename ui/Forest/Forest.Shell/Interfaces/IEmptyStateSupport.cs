namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IEmptyStateSupport
    {
        /// <summary>
        /// 当前状态为空
        /// </summary>
        bool IsEmpty { get; set; }

        /// <summary>
        /// 当前状态不为空
        /// </summary>
        bool NotEmpty { get; set; }

        /// <summary>
        /// 指示当前状态
        /// </summary>
        bool IsEmptyState { get; }
        
        /// <summary>
        /// 空状态
        /// </summary>
        object EmptyState { get; set; }
    }
}