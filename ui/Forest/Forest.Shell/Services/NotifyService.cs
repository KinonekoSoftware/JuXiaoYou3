using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class NotifyService : INotifyService, INotifyServiceAmbient
    {
        private const string Color = "#6F0240";
        private DialogHost _host;      
        private void RunOnDispatcherThread(Action action)
        {
            var dispatcher = _host.Dispatcher;
            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.Invoke(action);
        }

        /// <summary>
        /// 设置服务响应者
        /// </summary>
        /// <param name="host"></param>
        public void SetServiceProvider(DialogHost host) => _host = host;

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="notification">消息</param>
        public void Notify(IconNotification notification)
        {
            if (notification is null)
            {
                return;
            }

            if (notification.Delay.TotalMilliseconds < 1000)
            {
                //
                //
                notification.Initialize();
            }
            
            RunOnDispatcherThread(() => _host.Messaging(notification));
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="notification">消息</param>
        public void Notify(ImageNotification notification)
        {
            if (notification is null)
            {
                return;
            }

            if (notification.Delay.TotalMilliseconds < 1000)
            {
                //
                //
                notification.Initialize();
            }
            
            RunOnDispatcherThread(() => _host.Messaging(notification));
        }
    }
}