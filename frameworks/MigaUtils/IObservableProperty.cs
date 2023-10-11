using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaUtils
{
    /// <summary>
    /// 可观察属性。
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    public interface IObservableProperty<out T> : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// 
        /// </summary>
        void Dispose();
        
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
    
}