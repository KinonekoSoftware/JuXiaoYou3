using System.Collections.Generic;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class SettingListBox<T>: SettingItem<T>
    {
        /// <summary>
        /// 集合
        /// </summary>
        public IEnumerable<T> Collection { get; init; }
    }
}