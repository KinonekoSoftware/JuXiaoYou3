

namespace Acorisoft.FutureGL.Forest.Models
{
    /// <summary>
    /// 表示一个程序基本的应用设置。
    /// </summary>
    public class BasicAppSetting : ForestObject
    {
        private CultureArea _language;
        private MainTheme   _theme;

        /// <summary>
        /// 主题色
        /// </summary>
        public MainTheme Theme
        {
            get => _theme;
            set => SetValue(ref _theme, value);
        }

        /// <summary>
        /// 语言
        /// </summary>
        public CultureArea Language
        {
            get => _language;
            set => SetValue(ref _language, value);
        }
    }
}