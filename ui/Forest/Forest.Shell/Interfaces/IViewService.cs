using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    /// <summary>
    /// <see cref="IViewHostService"/> 表示视图服务模型
    /// </summary>
    public interface IViewService
    {
        /// <summary>
        /// 跳转到指定的页面
        /// </summary>
        /// <typeparam name="TViewModel">指定的视图模型</typeparam>
        /// <returns>返回操作结果</returns>
        void Route<TViewModel>() where TViewModel : IViewModel;

        /// <summary>
        /// 跳转到指定的页面
        /// </summary>
        /// <param name="parameter">跳转参数</param>
        /// <typeparam name="TViewModel">指定的视图模型</typeparam>
        /// <returns>返回操作结果</returns>
        void Route<TViewModel>(RoutingEventArgs parameter) where TViewModel : IViewModel;

        /// <summary>
        /// 跳转到指定的页面
        /// </summary>
        /// <param name="viewModel">指定的视图模型</param>
        /// <returns>返回操作结果</returns>
        void Route(IViewModel viewModel);

        /// <summary>
        /// 跳转到指定的页面
        /// </summary>
        /// <param name="parameter">跳转参数</param>
        /// <param name="viewModel">指定的视图模型</param>
        /// <returns>返回操作结果</returns>
        void Route(IViewModel viewModel, RoutingEventArgs parameter);
    }

    public interface IViewServiceAmbient : IServiceAmbient<ViewHostBase>
    {
    }
}