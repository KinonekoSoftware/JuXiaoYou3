using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class AdvancedSettingModel : ObservableObject
    {
        private int _debugMode;
        private int          _autoSavePeriod;

        /// <summary>
        /// 获取或设置 <see cref="AutoSavePeriod"/> 属性。
        /// </summary>
        public int AutoSavePeriod
        {
            get => _autoSavePeriod;
            set => SetValue(ref _autoSavePeriod, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="DebugMode"/> 属性。
        /// </summary>
        public int DebugMode
        {
            get => _debugMode;
            set => SetValue(ref _debugMode, value);
        }
    }
}