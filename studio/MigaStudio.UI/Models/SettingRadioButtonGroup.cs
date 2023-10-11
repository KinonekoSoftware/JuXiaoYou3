using System.Collections.Generic;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class SettingRadioButtonGroup: SettingItem<bool>
    {
        /// <summary>
        /// 集合
        /// </summary>
        public IEnumerable<SettingRadioButton> Collection { get; init; }
    }
}