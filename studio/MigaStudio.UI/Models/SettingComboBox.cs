using System.Collections.Generic;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public interface ISettingComboBox : ISettingItem
    {
        IEnumerable<object> Collection { get; }
        object Value { get; set; }
    }
    
    public class SettingComboBox<T> : ObservableObject, ISettingComboBox
    {
        private object _value;
        
        /// <summary>
        /// 集合
        /// </summary>
        public IEnumerable<object> Collection { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                SetValue(ref _value, value);
                Callback?.Invoke((T)value);
            }
        }

        public Action<T> Callback { get; set; }
        public string MainTitle { get; init; }
        public string SubTitle { get; init; }
    }
}