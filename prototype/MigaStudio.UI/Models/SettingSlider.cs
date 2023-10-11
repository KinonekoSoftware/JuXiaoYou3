namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class SettingSlider : SettingItem<int>
    {
        private int    _maximum;
        private int    _minimum;
        private string _unit;

        /// <summary>
        /// 获取或设置 <see cref="Unit"/> 属性。
        /// </summary>
        public string Unit
        {
            get => _unit;
            set => SetValue(ref _unit, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Minimum"/> 属性。
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set => SetValue(ref _minimum, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Maximum"/> 属性。
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }
    }
}