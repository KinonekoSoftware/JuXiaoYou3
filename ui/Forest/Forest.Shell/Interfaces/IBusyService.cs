using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    /// <summary>
    /// <see cref="IBusyService"/> 表示一个忙碌服务。
    /// </summary>
    public interface IBusyService
    {
        /// <summary>
        /// 创建会话
        /// </summary>
        /// <returns>返回一个忙碌会话。</returns>
        IBusySession CreateSession();
    }

    public interface IBusyServiceAmbient : IServiceAmbient<DialogHost>
    {
    }
    
    /// <summary>
    /// 表示一个忙碌状态的会话。
    /// </summary>
    public interface IBusySession : IDisposable
    {
        /// <summary>
        /// 更新内容并尝试开始会话。
        /// </summary>
        /// <remarks>如果已经开始会话则只会更新内容</remarks>
        /// <param name="text">要更新的内容</param>
        void Update(string text);

        /// <summary>
        /// 等待
        /// </summary>
        /// <returns></returns>
        Task Await();

        Task Await(string text);
    }
}