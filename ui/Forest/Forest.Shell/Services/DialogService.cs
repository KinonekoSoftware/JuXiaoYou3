using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.Forest.Views;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class DialogService : IDialogService, IDialogServiceAmbient, IBusyService, IBusyServiceAmbient, INotifyService, INotifyServiceAmbient
    {
        class Session : IBusySession
        {
            private void RunOnDispatcherThread(Action action)
            {
                var dispatcher = Host.Dispatcher;
                if (dispatcher.CheckAccess())
                    action();
                else
                    dispatcher.Invoke(action);
            }

            public Task Await() => Task.Delay(100);
            
            public Task Await(string text)
            {
                Update(text);
                return Task.Delay(100);  
            } 
            
            public void Dispose()
            {
                if (Host is null)
                {
                    return;
                }

                RunOnDispatcherThread(() => { Host.IsBusy = false; });

                Clean();
            }

            public void Update(string text)
            {
                if (Host is null)
                {
                    return;
                }

                RunOnDispatcherThread(() =>
                {
                    //
                    // 更新
                    Host.BusyText = text;

                    if (!Host.IsBusy)
                    {
                        Host.IsBusy = true;
                    }
                });
            }

            public DialogHost Host { get; init; }
            public Action Clean { get; init; }
        }

        private const string Color = "#6F0240";

        private Session    _session;
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

            if (notification.Delay
                            .TotalMilliseconds < 1000)
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

        /// <summary>
        /// 创建会话。
        /// </summary>
        /// <returns>返回会话</returns>
        public IBusySession CreateSession()
        {
            _session ??= new Session
            {
                Host  = _host,
                Clean = () => _session = null,
            };


            return _session;
        }

        #region IDialogAmbientService

        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public Task Notify(CriticalLevel level, string title, string content)
        {
            return Notify(level, title, content, Language.ConfirmText);
        }
        
        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seconds">倒计时</param>
        /// <returns></returns>
        public Task Notify(CriticalLevel level, string title, string content, int seconds)
        {
            return Notify(level, title, content,seconds, Language.ConfirmText);
        }

        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="buttonText">确认按钮文本</param>
        /// <returns></returns>
        public async Task Notify(CriticalLevel level, string title, string content, string buttonText)
        {
            DialogViewModel dvm;

            if (level == CriticalLevel.Danger)
            {
                dvm = new DangerNotifyViewModel
                {
                    Title            = title,
                    Content          = content,
                    CompleteButtonText = buttonText,
                };
            }
            else if (level == CriticalLevel.Warning)
            {
                dvm = new WarningNotifyViewModel
                {
                    Title              = title,
                    Content            = content,
                    CompleteButtonText = buttonText,
                };
            }
            else if (level == CriticalLevel.Info)
            {
                dvm = new InfoViewModel
                {
                    Title              = title,
                    Content            = content,
                    CompleteButtonText = buttonText,
                };
            }
            else if (level == CriticalLevel.Success)
            {
                dvm = new SuccessViewModel
                {
                    Title              = title,
                    Content            = content,
                    CompleteButtonText = buttonText,
                };
            }
            else
            {
                dvm = new ObsoleteViewModel
                {
                    Title   = title,
                    Content = content,
                    CompleteButtonText = buttonText
                };
            }

            await _host.ShowDialog(dvm, new RoutingEventArgs());
        }
        
        /// <summary>
        /// 通知对话框
        /// </summary>
        /// <param name="level">标题</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seconds">倒计时</param>
        /// <param name="buttonText">确认按钮文本</param>
        /// <returns></returns>
        public async Task Notify(CriticalLevel level, string title, string content,int seconds, string buttonText)
        {
            DialogViewModel dvm;

            if (level == CriticalLevel.Danger)
            {
                dvm = new DangerNotifyViewModel
                {
                    Title            = title,
                    Content          = content,
                    CompleteButtonText = buttonText,
                    CountDown        = true,
                    CountSeconds     = seconds
                };
            }
            else if (level == CriticalLevel.Warning)
            {
                dvm = new WarningNotifyViewModel
                {
                    Title              = title,
                    Content            = content,
                    CompleteButtonText = buttonText,
                    CountDown          = true,
                    CountSeconds       = seconds
                };
            }
            else if (level == CriticalLevel.Info)
            {
                dvm = new InfoViewModel
                {
                    Title              = title,
                    Content            = content,
                    CompleteButtonText = buttonText,
                    CountDown          = true,
                    CountSeconds       = seconds
                };
            }
            else if (level == CriticalLevel.Success)
            {
                dvm = new SuccessViewModel
                {
                    Title              = title,
                    Content            = content,
                    CompleteButtonText = buttonText,
                    CountDown          = true,
                    CountSeconds       = seconds
                };
            }
            else
            {
                dvm = new ObsoleteViewModel
                {
                    Title        = title,
                    CompleteButtonText = buttonText,
                    Content      = content,
                    CountDown    = true,
                    CountSeconds = seconds
                };
            }

            await _host.ShowDialog(dvm, new RoutingEventArgs());
        }

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Danger(string title, string content)
        {
            return Danger(title, content, Language.ConfirmText, Language.CancelText);
        }
        
        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <param name="seconds">倒计时</param>
        /// <returns></returns>
        public Task<bool> Danger(string title, string content,int seconds)
        {
            return Danger(title, content, seconds, Language.ConfirmText, Language.CancelText);
        }

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Danger(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new DangerViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new RoutingEventArgs());
            return result.IsFinished;
        }
        
        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <param name="seconds">倒计时</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Danger(string title, string content,int seconds, string okButtonText, string cancelButtonText)
        {
            var dvm = new DangerViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
                CountDown = true,
                CountSeconds = seconds
            };

            var result = await _host.ShowDialog(dvm, new RoutingEventArgs());
            return result.IsFinished;
        }

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Warning(string title, string content)
        {
            return Warning(title, content, Language.ConfirmText, Language.CancelText);
        }
        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seconds">倒计时</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Warning(string title, string content, int seconds)
        {
            return Warning(title, content, seconds, Language.ConfirmText, Language.CancelText);
        }
        
        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="seconds">倒计时</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Warning(string title, string content,int seconds, string okButtonText, string cancelButtonText)
        {
            var dvm = new WarningViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
                CountDown = true,
                CountSeconds = seconds
            };

            var result = await _host.ShowDialog(dvm, new RoutingEventArgs());
            return result.IsFinished;
        }
        

        /// <summary>
        /// 危险提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Warning(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new WarningViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new RoutingEventArgs());
            return result.IsFinished;
        }

        /// <summary>
        /// 信息提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        public Task<bool> Info(string title, string content)
        {
            return Info(title, content, Language.ConfirmText, Language.CancelText);
        }

        /// <summary>
        /// 信息提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Info(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new InfoViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new RoutingEventArgs());
            return result.IsFinished;
        }


        /// <summary>
        /// 成功提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Success(string title, string content)
        {
            return Success(title, content, Language.ConfirmText, Language.CancelText);
        }


        /// <summary>
        /// 成功提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Success(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new SuccessViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new RoutingEventArgs());
            return result.IsFinished;
        }

        /// <summary>
        /// 过时提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回一个可等待的任务。</returns>
        /// <returns></returns>
        public Task<bool> Obsolete(string title, string content)
        {
            return Obsolete(title, content, Language.ConfirmText, Language.CancelText);
        }

        /// <summary>
        /// 过时提示对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="okButtonText">确认按钮文本</param>
        /// <param name="cancelButtonText">放弃按钮文本</param>
        /// <returns>返回一个可等待的任务。</returns>
        public async Task<bool> Obsolete(string title, string content, string okButtonText, string cancelButtonText)
        {
            var dvm = new ObsoleteViewModel
            {
                Title              = title,
                Content            = content,
                CancelButtonText   = cancelButtonText,
                CompleteButtonText = okButtonText,
            };

            var result = await _host.ShowDialog(dvm, new RoutingEventArgs());
            return result.IsFinished;
        }

        #endregion

        #region IDialogService

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T, TViewModel>() where TViewModel : IDialogViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>() ?? Classes.CreateInstance<TViewModel>();
            return _host.ShowDialog<T>(vm, RoutingEventArgs.Empty);
        }

        public Task<Op<T>> Dialog<T, TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel
        {
            return _host.ShowDialog<T>(viewModel, RoutingEventArgs.Empty);
        }


        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T, TViewModel>(Parameter parameter) where TViewModel : IDialogViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>() ?? Classes.CreateInstance<TViewModel>();
            return _host.ShowDialog<T>(vm, new RoutingEventArgs
            {
                Parameter = parameter
            });
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="TViewModel">视图模型类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T, TViewModel>(TViewModel viewModel, Parameter parameter) where TViewModel : IDialogViewModel
        {
            return _host.ShowDialog<T>(viewModel, new RoutingEventArgs
            {
                Parameter = parameter
            });
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T>(IDialogViewModel viewModel)
        {
            return _host.ShowDialog<T>(viewModel, RoutingEventArgs.Empty);
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<T>> Dialog<T>(IDialogViewModel viewModel, Parameter parameter)
        {
            return _host.ShowDialog<T>(viewModel, new RoutingEventArgs
            {
                Parameter = parameter
            });
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<object>> Dialog(IDialogViewModel viewModel)
        {
            return _host.ShowDialog(viewModel, RoutingEventArgs.Empty);
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="viewModel">视图模型实例</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回一个可等待的任务</returns>
        public Task<Op<object>> Dialog(IDialogViewModel viewModel, Parameter parameter)
        {
            return _host.ShowDialog(viewModel, new RoutingEventArgs
            {
                Parameter = parameter
            });
        }


        public void CloseAll() => _host.CloseDialog();
        public bool IsOpened() => _host.IsDialogOpened();
        public bool IsOpened(IDialogViewModel viewModel) => _host.IsDialogOpened(viewModel);

        public bool IsOpened<TViewModel>() where TViewModel : IDialogViewModel => _host.IsDialogOpened<TViewModel>();

        #endregion
    }
}