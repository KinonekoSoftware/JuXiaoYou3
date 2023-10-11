
namespace Acorisoft.FutureGL.Forest.Interfaces
{
    
    public interface ITabViewModel :IRootViewModel, IApprovalRequired
    {
        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="windowKeyEventArgs">键盘参数</param>
        void SetWindowEvent(WindowKeyEventArgs windowKeyEventArgs);
        
        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="windowKeyEventArgs">拖拽参数</param>
        void SetWindowEvent(WindowDragDropArgs windowKeyEventArgs);
        
        /// <summary>
        /// 唯一标识符。
        /// </summary>
        string PageId { get; }
        
        /// <summary>
        /// 标题。
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// 唯一性
        /// </summary>
        bool Uniqueness { get; }
        
        /// <summary>
        /// 是否可被关闭
        /// </summary>
        bool Removable { get; }
    }
}