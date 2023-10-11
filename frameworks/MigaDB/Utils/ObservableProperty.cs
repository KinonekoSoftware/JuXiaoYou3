using System.Reactive.Subjects;

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public class ObservableProperty<T> : NotifyPropertyChanged, IObservableProperty<T>
    {
        private readonly BehaviorSubject<T> _stream;
        private          T                  _value;

        public ObservableProperty()
        {
            _value  = default(T);
            _stream = new BehaviorSubject<T>(_value);
        }

        public ObservableProperty(T defaultValue)
        {
            _value  = defaultValue;
            _stream = new BehaviorSubject<T>(defaultValue);
        }

        public void SetValue(T value)
        {
            _value = value;
            _stream.OnNext(value);
            ValueChanged?.Invoke(value);
        }

        public void Dispose()
        {
            // 释放
            (_value as IDisposable)?.Dispose();
            SetValue(_value);
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public T CurrentValue => _value;
        
        /// <summary>
        /// 可观测对象
        /// </summary>
        public IObservable<T> Observable => _stream;
        
        /// <summary>
        /// 值改变事件。
        /// </summary>
        public event Action<T> ValueChanged;
    }
}