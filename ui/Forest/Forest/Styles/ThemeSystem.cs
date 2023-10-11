
namespace Acorisoft.FutureGL.Forest.Styles
{
    /// <summary>
    /// <see cref="ThemeSystem"/> 类型表示一个样式系统。
    /// </summary>
    public class ThemeSystem : ForestObject
    {
        /// <summary>
        /// 当前的主题系统。
        /// </summary>
        public static ThemeSystem Instance { get; } = new ThemeSystem();
        
        /// <summary>
        /// 当前的主题。
        /// </summary>
        public ForestThemeSystem Theme { get; set; }
    }
}