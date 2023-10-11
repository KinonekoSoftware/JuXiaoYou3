namespace Acorisoft.FutureGL.Forest
{
    public interface ILifetimeSupport
    {
        /// <summary>
        /// 首次启动
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        void Start();

        /// <summary>
        /// 表示关闭
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        void Stop();
    }
}