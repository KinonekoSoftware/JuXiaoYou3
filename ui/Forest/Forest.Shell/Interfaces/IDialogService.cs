using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public enum CriticalLevel
    {
        Danger,
        Warning,
        Info,
        Success,
        Obsoleted
    }
    
    /// <summary>
    /// <see cref="IDialogService"/> 类型表示一个对话框服务
    /// </summary>
    public interface IDialogService : IBuiltinDialogService
    {
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<T>> Dialog<T, TViewModel>() where TViewModel : IDialogViewModel;
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<T>> Dialog<T, TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel;
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<T>> Dialog<T, TViewModel>(Parameter parameter) where TViewModel : IDialogViewModel;
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<T>> Dialog<T, TViewModel>(TViewModel viewModel, Parameter parameter) where TViewModel : IDialogViewModel;
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<T>> Dialog<T>(IDialogViewModel viewModel);
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<T>> Dialog<T>(IDialogViewModel viewModel, Parameter parameter);
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<object>> Dialog(IDialogViewModel viewModel);
        
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        Task<Op<object>> Dialog(IDialogViewModel viewModel, Parameter parameter);

        /// <summary>
        /// 关闭所有对话框
        /// </summary>
        void CloseAll();

        /// <summary>
        /// 判断是否已经打开对话框
        /// </summary>
        /// <returns>如果打开返回true，否则返回false。</returns>
        bool IsOpened();

        bool IsOpened<TViewModel>() where TViewModel : IDialogViewModel;

        /// <summary>
        /// 判断是否已经打开对话框
        /// </summary>
        /// <param name="viewModel">要判断的对话框视图模型实例</param>
        /// <returns>如果打开返回true，否则返回false。</returns>
        bool IsOpened(IDialogViewModel viewModel);
    }

    /// <summary>
    /// <see cref="IBuiltinDialogService"/> 类型表示一个环境服务
    /// </summary>
    public interface IBuiltinDialogService
    {

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Danger(string title, string content);
        
        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seconds">倒计时</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Danger(string title, string content, int seconds);

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Danger(string title, string content, string okButtonText, string cancelButtonText);
        
        /// <summary>
        /// 警告提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Warning(string title, string content);
        
        
        /// <summary>
        /// 警告提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seconds">倒计时</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Warning(string title, string content, int seconds);
        
        /// <summary>
        /// 警告提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        Task<bool> Warning(string title, string content, string okButtonText, string cancelButtonText);

        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        Task Notify(CriticalLevel level, string title, string content);
        
        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seconds">倒计时</param>
        /// <returns></returns>
        Task Notify(CriticalLevel level, string title, string content, int seconds);

        Task Notify(CriticalLevel level, string title, string content, string buttonText);
        Task Notify(CriticalLevel level, string title, string content, int seconds, string buttonText);
    }

    /// <summary>
    /// <see cref="IDialogServiceAmbient"/> 类型表示一个对话框环境服务
    /// </summary>
    public interface IDialogServiceAmbient : IServiceAmbient<DialogHost>
    {
        
    }
}