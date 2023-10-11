using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest.Exceptions;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.Controls
{
    /// <summary>
    /// <see cref="DialogHost"/> 类型表示一个对话框宿主。
    /// </summary>
    public class DialogHost : ContentControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ViewModelProperty;
        public static readonly DependencyProperty DialogProperty;
        public static readonly DependencyProperty MessageProperty;
        public static readonly DependencyProperty IsOpenedProperty;
        public static readonly DependencyProperty IsBusyProperty;
        public static readonly DependencyProperty BusyTextProperty;
        public static readonly RoutedEvent        NotificationOpeningEvent;
        public static readonly RoutedEvent        NotificationClosingEvent;

        #endregion

        static DialogHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogHost),
                                                     new FrameworkPropertyMetadata(typeof(DialogHost)));

            IsBusyProperty = DependencyProperty.Register(nameof(IsBusy),
                                                         typeof(bool),
                                                         typeof(DialogHost),
                                                         new PropertyMetadata(Boxing.False));

            MessageProperty = DependencyProperty.Register(nameof(Message),
                                                          typeof(Notification),
                                                          typeof(DialogHost),
                                                          new PropertyMetadata(null));

            BusyTextProperty = DependencyProperty.Register(nameof(BusyText),
                                                           typeof(string),
                                                           typeof(DialogHost),
                                                           new PropertyMetadata("正在加载..."));

            IsOpenedProperty = DependencyProperty.Register(nameof(IsOpened),
                                                           typeof(bool),
                                                           typeof(DialogHost),
                                                           new PropertyMetadata(Boxing.False));


            ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
                                                            typeof(IViewModel),
                                                            typeof(DialogHost),
                                                            new PropertyMetadata(default(IViewModel),
                                                                                 OnViewModelChanged));

            DialogProperty = DependencyProperty.Register(nameof(Dialog),
                                                         typeof(object),
                                                         typeof(DialogHost),
                                                         new PropertyMetadata(default(object)));

            NotificationOpeningEvent = EventManager.RegisterRoutedEvent(nameof(NotificationOpening), RoutingStrategy.Bubble,
                                                                        typeof(RoutedEventHandler),
                                                                        typeof(DialogHost));

            NotificationClosingEvent = EventManager.RegisterRoutedEvent(nameof(NotificationClosing), RoutingStrategy.Bubble,
                                                                        typeof(RoutedEventHandler),
                                                                        typeof(DialogHost));
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = (DialogHost)d;

            if (e.NewValue is IDialogViewModel newValue)
            {
                var view = Xaml.Connect(newValue);

                if (view is not FrameworkElement fe)
                {
                    if (host._stack.CanCompleteDialog())
                        host._stack.CompleteDialog();
                    return;
                }

                if (view is ForestUserControl ph)
                {
                    ph.DataContext = newValue;
                }
                else
                {
                    fe.Loaded += OnDialogContentLoaded;
                }

                //
                // 设置标题
                if (string.IsNullOrEmpty(newValue.Title))
                {
                    newValue.Title = Services.Language
                                             .GetTypeName(newValue.GetType());
                }
                
                //
                // 设置视图
                host.Dialog   = view;
                host.IsOpened = true;
            }
            else
            {
                host.ClearValue(DialogProperty);
                host.ClearValue(ViewModelProperty);
            }
        }

        private static void OnDialogContentLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement fe)
            {
                return;
            }

            if (fe is not ForestUserControl && fe.DataContext is IViewModel vm)
            {
                vm.Start();
                
                //
                // 注意：在第一次执行Loaded之后就要取消事件订阅
                fe.Loaded -= OnDialogContentLoaded;
            }
        }

        private readonly DialogStack                   _stack;
        private readonly ConcurrentQueue<Notification> _queue;
        private readonly DispatcherTimer               _timer;

        private int  _messageCounting;
        private int  _messageDelay;
        private int  _animationCounting;
        private bool _hasMessagePending;
        private int  Milliseconds;


        public DialogHost()
        {
            Milliseconds  =  10;
            _stack        =  new DialogStack();
            _queue        =  new ConcurrentQueue<Notification>();
            _timer        =  new DispatcherTimer(TimeSpan.FromMilliseconds(Milliseconds), DispatcherPriority.Normal, OnDispatchMessage, Dispatcher);
            Unloaded += OnUnloaded;

            Xaml.Get<IDialogServiceAmbient>().SetServiceProvider(this);
            Xaml.Get<IBusyServiceAmbient>().SetServiceProvider(this);
            Xaml.Get<INotifyServiceAmbient>().SetServiceProvider(this);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _queue.Clear();
        }

        private void HandlingNotificationPending()
        {
            _messageCounting += Milliseconds;

            //
            // 不满足时。
            if (_messageCounting - Milliseconds < _messageDelay)
            {
                return;
            }

            //
            // 满足时
            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = NotificationClosingEvent,
            });

            _messageCounting   = 0;
            _animationCounting = 300 / Milliseconds;
            _hasMessagePending = false;
        }

        private void OnDispatchMessage(object sender, EventArgs e)
        {
            if (_hasMessagePending)
            {
                HandlingNotificationPending();
                return;
            }

            //
            // 已经空了就退出
            if (_queue.IsEmpty)
            {
                Message = null;
                _timer.Stop();
                return;
            }

            //
            // 动画计时
            if (_animationCounting > 0)
            {
                _animationCounting--;
                return;
            }


            //
            // 尝试弹出队列
            if (!_queue.TryDequeue(out var message))
            {
                return;
            }

            Message            = message;
            Milliseconds       = 50;
            _hasMessagePending = true;
            _messageCounting   = 0;
            _messageDelay      = (int)message.Delay.TotalMilliseconds;
            _timer.Interval    = TimeSpan.FromMilliseconds(Milliseconds);
            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = NotificationOpeningEvent,
            });
        }

        public void CloseAll()
        {
            while (_stack.CanCompleteDialog())
            {
                CloseDialog();
            }
        }
        
        public bool IsDialogOpened() => _stack.CanCompleteDialog();

        public bool IsDialogOpened<TViewModel>() where TViewModel : IDialogViewModel => _stack.Contains<TViewModel>();
        
        public bool IsDialogOpened(IDialogViewModel viewModel) => viewModel is not null && _stack.Contains(viewModel);

        public async Task<Op<T>> ShowDialog<T>(IDialogViewModel dialog, RoutingEventArgs param)
        {
            var result = await ShowDialog(dialog, param);

            if (result is null)
            {
                return Op<T>.Failed("参数返回空");
            }

            return result.IsFinished ? Op<T>.Success((T)result.Value) : Op<T>.Failed(result.Reason);
        }

        public async Task<Op<object>> ShowDialog(IDialogViewModel dialog, RoutingEventArgs param)
        {
            if (dialog is null)
            {
                return Op<object>.Failed("视图模型为空");
            }

            try
            {
                //
                // 弹出
                var previous = _stack.WaitForOpen(dialog);

                if (dialog is not __IDialogViewModel dialog2)
                {
                    return Op<object>.Failed("这不是一个可等待的对话框");
                }

                //
                // 暂停
                if (dialog != previous)
                {
                    previous.Suspend();
                }


                //
                // 设置参数。
                param                 ??= new RoutingEventArgs();
                param.ViewModelSource =   previous;
                param.CloseHandler    =   CloseDialog;
                dialog2.CloseHandler  =   CloseDialog;

                //
                //
                dialog.Startup(param);


                //
                //
                Dispatcher.Invoke(() =>
                {
                    Focus();
                    return ViewModel = dialog;
                });
                
                //
                // 等待
                return await dialog2.WaitHandle.Task;
            }
            catch (DuplicateDataException)
            {
                return Op<object>.Failed("对话框重复打开！");
            }
        }

        public void Messaging(Notification message)
        {
            if (message is null)
            {
                return;
            }

            _queue.Enqueue(message);
            _timer.Start();
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <remarks>注意：该操作不会设置返回值。也不会取消</remarks>
        public void CloseDialog()
        {
            Dispatcher.Invoke(() =>
            {
                if (_stack.Current is null)
                {
                    return;
                }

                //
                //
                var finished = _stack.CompleteDialog();

                //
                //
                finished.Stop();

                //
                // 恢复上一个
                if (!_stack.CanCompleteDialog())
                {
                    IsOpened  = false;
                    ViewModel = null;
                    return;
                }

                //
                //
                if (finished is not __IDialogViewModel dialog2)
                {
                    IsOpened = false;
                    return;
                }

                //
                // 如果对话框还没关闭，则直接尝试取消
                dialog2.WaitHandle.TrySetCanceled();

                //
                // 恢复上下文
                var current = _stack.Current;

                //
                //
                current.Resume();

                //
                // 弹出
                IsOpened  = false;
                ViewModel = current;
            });
        }

        /// <summary>
        /// 视图模型
        /// </summary>
        public IViewModel ViewModel
        {
            get => (IViewModel)GetValue(ViewModelProperty);
            internal set => SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// 实际对话框内容
        /// </summary>
        public object Dialog
        {
            get => GetValue(DialogProperty);
            private set => SetValue(DialogProperty, value);
        }

        public bool IsOpened
        {
            get => (bool)GetValue(IsOpenedProperty);
            set => SetValue(IsOpenedProperty, Boxing.Box(value));
        }

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, Boxing.Box(value));
        }

        public string BusyText
        {
            get => (string)GetValue(BusyTextProperty);
            set => SetValue(BusyTextProperty, value);
        }

        public Notification Message
        {
            get => (Notification)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public event RoutedEventHandler NotificationOpening
        {
            add => AddHandler(NotificationOpeningEvent, value);
            remove => RemoveHandler(NotificationOpeningEvent, value);
        }

        public event RoutedEventHandler NotificationClosing
        {
            add => AddHandler(NotificationClosingEvent, value);
            remove => RemoveHandler(NotificationClosingEvent, value);
        }

        public class DialogStack
        {
            private readonly Stack<IDialogViewModel> _waitingStack = new Stack<IDialogViewModel>(16);
            private readonly HashSet<int>            _duplication  = new HashSet<int>();

            private IDialogViewModel _current;
            private IDialogViewModel _previous;

            /// <summary>
            /// 等待打开对话框
            /// </summary>
            /// <param name="dialog">要打开的对话框</param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException">参数为空</exception>
            /// <exception cref="DuplicateDataException">对话框重复加载</exception>
            public IDialogViewModel WaitForOpen(IDialogViewModel dialog)
            {
                if (dialog is null)
                {
                    throw new ArgumentNullException(nameof(dialog));
                }

                if (!_duplication.Add(dialog.GetHashCode()))
                {
                    throw new DuplicateDataException(nameof(dialog));
                }

                if (_current is null)
                {
                    _current = dialog;
                    return _current;
                }

                _previous = _current;
                _current  = dialog;
                _waitingStack.Push(_previous);

                return _previous;
            }

            /// <summary>
            /// 完成或者取消
            /// </summary>
            /// <returns>返回完成的值。</returns>
            public IDialogViewModel CompleteDialog()
            {
                //
                // 注意：
                // _previous 和 _waitingStack的栈顶是等价的
                var finished = _current;

                if (_waitingStack.Count > 0)
                {
                    //
                    _previous = _waitingStack.Pop();
                    _current  = _previous;
                }
                else
                {
                    _previous = null;
                    _current  = null;
                }

                _duplication.Remove(finished.GetHashCode());

                return finished;
            }

            /// <summary>
            /// 是否可以完成或者取消
            /// </summary>
            /// <returns>返回一个值</returns>
            public bool CanCompleteDialog() => _waitingStack.Count > 0 || _current is not null;

            /// <summary>
            /// 当前的值
            /// </summary>
            public IDialogViewModel Current => _current;

            /// <summary>
            /// 之前的值
            /// </summary>
            public IDialogViewModel Previous => _previous;

            public bool Contains(IDialogViewModel viewModel) => ReferenceEquals(_current, viewModel) ||
                                                                _waitingStack.Any(x => ReferenceEquals(x, viewModel));
            public bool Contains<TViewModel>() where TViewModel : IDialogViewModel => _waitingStack.Any(x => x is TViewModel);
        }
    }
}