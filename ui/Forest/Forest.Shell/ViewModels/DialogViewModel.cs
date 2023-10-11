using System.ComponentModel;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;
using DialogTaskSource = System.Threading.Tasks.TaskCompletionSource<Acorisoft.FutureGL.Forest.Op<object>>;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    /// <summary>
    /// <see cref="DialogViewModel"/> 类型表示一个对话框视图模型基类。
    /// </summary>
    public abstract class DialogViewModel : ViewModelBase, IDialogViewModel, __IDialogViewModel
    {
        protected readonly DialogTaskSource Wait;
        protected          Action           CloseAction;
        private            string           _title;
        protected          IDisposable      WindowEventBroadcastDisposable;

        protected DialogViewModel()
        {
            WindowEventBroadcastDisposable = Xaml.Get<IWindowEventBroadcast>()
                                              .Keys
                                              .Subscribe(OnKeyboardInput);

            CancelCommand = new RelayCommand(Cancel);
            Wait          = new DialogTaskSource();
        }

        public override void Stop()
        {
            WindowEventBroadcastDisposable.Dispose();
            base.Stop();
        }

        protected virtual void OnKeyboardInput(WindowKeyEventArgs e)
        {
        }
        
        

        /// <summary>
        /// 传递参数。
        /// </summary>
        /// <param name="parameter">指定要传递的参数。</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override void OnStartup(RoutingEventArgs parameter)
        {
            if (parameter is null)
            {
                return;
            }

            OnStart(parameter);
        }

        /// <summary>
        /// 传递参数。
        /// </summary>
        /// <param name="parameter">指定要传递的参数。</param>
        protected virtual void OnStart(RoutingEventArgs parameter)
        {
        }

        /// <summary>
        /// 取消当前操作。
        /// </summary>
        public virtual void Cancel()
        {
            if (CloseAction is null)
            {
                return;
            }

            if (Wait.TrySetResult(Op<object>.Failed("手动取消")))
            {
                //
                // 清理现场
                CloseAction();
                CloseAction = null;
            }
        }

        /// <summary>
        /// 表示一个等待句柄
        /// </summary>
        DialogTaskSource __IDialogViewModel.WaitHandle => Wait;

        /// <summary>
        /// 设置当前的清理方法
        /// </summary>
        Action __IDialogViewModel.CloseHandler
        {
            get => CloseAction;
            set => CloseAction = value;
        }
        
        public bool IsEditMode { get; protected set; }

        /// <summary>
        /// 取消命令。
        /// </summary>
        public RelayCommand CancelCommand { get; }

        /// <summary>
        /// 获取或设置标题。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }
    }
}