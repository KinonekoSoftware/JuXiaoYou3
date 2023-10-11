using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.AppModels
{
    public interface IViewController : IViewModel
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
        /// 
        /// </summary>
        WindowState WindowState { get; set; }
    }
}