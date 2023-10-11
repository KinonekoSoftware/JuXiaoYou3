namespace Acorisoft.FutureGL.Forest.Styles
{
    /// <summary>
    /// 用于表示一个视觉状态。
    /// </summary>
    public enum VisualState
    {
        /// <summary>
        /// 所有控件的初始状态
        /// </summary>
        Normal = 0,
        
        /// <summary>
        /// 高亮的视觉状态
        /// </summary>
        /// <remarks>一般用于MouseOver</remarks>
        Highlight1,
        
        /// <summary>
        /// 高亮的另一个视觉状态
        /// </summary>
        /// <remarks>一般用于Pressed、Selected</remarks>
        Highlight2,
        
        /// <summary>
        /// 数据控件的错误状态
        /// </summary>
        Error,
        
        /// <summary>
        /// 所有控件的活动状态。
        /// </summary>
        /// <remarks>一般用于SelectedInactive</remarks>
        Active,
        
        /// <summary>
        /// 所有控件的失效状态。
        /// </summary>
        /// <remarks>一般用于IsEnabled = false</remarks>
        Inactive
    }
}