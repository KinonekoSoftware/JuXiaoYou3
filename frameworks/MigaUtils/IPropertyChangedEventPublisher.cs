namespace Acorisoft.FutureGL.MigaUtils
{

    public interface IPropertyChangedEventPublisher
    {
        void RaiseUpdated(string name);
        void Unsubscribe(IPropertyChangedEventSubscriber subscriber);
        void Subscribe(IPropertyChangedEventSubscriber subscriber);
    }

    public interface IPropertyChangedEventSubscriber
    {
        /// <summary>
        /// 接收属性变更通知。
        /// </summary>
        /// <param name="name">变更的属性名。</param>
        void Receive(string name);
    }

}