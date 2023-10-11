using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface INotifyService
    {
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="notification">消息</param>
        void Notify(IconNotification notification);
        
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="notification">消息</param>
        void Notify(ImageNotification notification);
    }
    
    
    public interface INotifyServiceAmbient : IServiceAmbient<DialogHost>
    {
        
    }
}