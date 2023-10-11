namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public interface ISettingItem
    {
        string MainTitle { get; }
        string SubTitle { get; }
    }

    public interface ISettingItem<T> : ISettingItem
    {
        Action<T> Callback { get; set; }
        T Value { get; set; }
    }

    public abstract class SettingItem<T> : ObservableObject, ISettingItem<T>
    {
        private T _value;

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                SetValue(ref _value, value);
                Callback?.Invoke(value);
            }
        }

        public Action<T> Callback { get; set; }
        public string MainTitle { get; init; }
        public string SubTitle { get; init; }
    }
}