using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest;

namespace Acorisoft.FutureGL.Tools.MusicPlayer.Services
{
    /// <summary>
    /// 可观察属性。
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    public interface IObservableProperty<out T> : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        /// <summary>
        /// 当前值
        /// </summary>
        T CurrentValue { get; }

        /// <summary>
        /// 可观测
        /// </summary>
        IObservable<T> Observable { get; }

        /// <summary>
        /// 属性变更事件。
        /// </summary>
        event Action<T> ValueChanged;
    }
    
    
    public class ObservableProperty<T> : ForestObject, IObservableProperty<T>, IDisposable
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

        protected override void ReleaseManagedResources()
        {
            // 释放
            (_value as IDisposable)?.Dispose();
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