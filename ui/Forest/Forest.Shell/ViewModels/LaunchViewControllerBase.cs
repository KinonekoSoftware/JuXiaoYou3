using System.Reactive.Concurrency;
using System.Threading;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class LaunchViewControllerBase: ViewModelBase, ITabViewController
    {
        private          string _caption;
        private          object _context;
        private readonly object _sync;
        private          bool   _init;
        private          WindowState _windowState;
        
        protected LaunchViewControllerBase()
        {
            _sync   = new object();
            Jobs    = new Queue<AsyncJob>(32);
        }

        protected void Init()
        {
            _context ??= GetExecuteContext();
        }

        protected void Job(string id, Action<object> handler)
        {
            Jobs.Enqueue(new AsyncJob
            {
                Title = Language.GetText(id),
                Handler = handler
            });
        }

        private void AsyncJobProcess()
        {
            lock (_sync)
            {
                while (Jobs.Count > 0)
                {
                    var job = Jobs.Dequeue();

                    if (job?.Handler is null)
                    {
                        continue;
                    }
                
                    Scheduler.Schedule(() =>
                    {
                        Caption = job.Title;
                    });
                
                    job.Handler(_context);
                }
                
                Scheduler.Schedule(() =>
                {
                    Caption = "加载完成";
                    OnJobCompleted();
                });
                
            }
        }

        protected abstract object GetExecuteContext();

        public void Start(ITabViewModel viewModel){}

        protected virtual void OnJobCompleted()
        {
            
        }

        public sealed override void Start()
        {
            if (_init)
            {
                return;
            }

            _init = true;
            ThreadPool.QueueUserWorkItem(_ => AsyncJobProcess());
        }

        public void SetWindowEvent(WindowKeyEventArgs e)
        {
        }

        public void SetWindowEvent(WindowDragDropArgs e)
        {
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Caption"/> 属性。
        /// </summary>
        public string Caption
        {
            get => _caption;
            set => SetValue(ref _caption, value);
        }
        
        /// <summary>
        /// 所有
        /// </summary>
        public Queue<AsyncJob> Jobs { get; }

        /// <summary>
        /// 获取或设置 <see cref="WindowState"/> 属性。
        /// </summary>
        public WindowState WindowState
        {
            get => _windowState;
            set => SetValue(ref _windowState, value);
        }

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id => "::Launch";
    }
}