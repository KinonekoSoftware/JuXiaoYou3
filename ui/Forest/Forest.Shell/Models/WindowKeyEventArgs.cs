using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest.Models
{
    public class WindowKeyEventArgs : EventArgs
    {
        /// <summary>
        /// 事件参数
        /// </summary>
        public KeyEventArgs Args { get; init; }
        
        /// <summary>
        /// 是否为按下
        /// </summary>
        public bool IsDown { get; init; }
    }
}