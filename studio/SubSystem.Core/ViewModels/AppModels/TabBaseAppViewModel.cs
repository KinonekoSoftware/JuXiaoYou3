using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabBaseAppViewModel : AppViewModelBase
    {
        private GlobalStudioContext _context;
        private IViewController     _currentController;
        private bool                _initialized;

        protected TabBaseAppViewModel()
        {
            ApplicationModel = Xaml.Get<ApplicationModel>();
            var web = Xaml.Get<IWindowEventBroadcast>();
            
            web.Keys
               .Subscribe(OnWindowEventRouting)
               .DisposeWith(Collector);
            
            web.Drags
               .Subscribe(OnWindowEventRouting)
               .DisposeWith(Collector);
            
            web.PropertyTunnel
               .WindowState = x => CurrentController.WindowState = x;
        }

        protected virtual bool OnKeyDown(WindowKeyEventArgs e)
        {
            return false;
        }

        private void OnWindowEventRouting(WindowKeyEventArgs e)
        {
            if (e.IsDown || e.Args.IsRepeat)
            {
                return;
            }

            if (!OnKeyDown(e))
            {
                CurrentController?.SetWindowEvent(e);
            }
        }

        private void OnWindowEventRouting(WindowDragDropArgs e)
        {
            CurrentController?.SetWindowEvent(e);
        }

        protected void OnInitialize()
        {
            _initialized = true;
        }

        protected void OnInitialize(GlobalStudioContext context)
        {
            _context     = context;
            _initialized = true;
        }

        public sealed override void Start()
        {
            if (_initialized)
            {
                StartOverride();
            }
        }

        public sealed override void Stop()
        {
            _context.Controllers
                    .ForEach(x => x.Stop());
            StopOverride();
        }
        
        protected virtual void StopOverride()
        {
        }

        protected virtual void StartOverride()
        {
        }

        protected virtual void OnControllerChanged(IViewController oldController, IViewController newController)
        {
        }

        /// <summary>
        /// 应用程序模型
        /// </summary>
        public ApplicationModel ApplicationModel { get; }

        /// <summary>
        /// 表示当前的主要视图控制器。
        /// </summary>
        /// <remarks>
        /// 该属性必须为 <see cref="ITabViewController"/> 或者 <see cref="ISingleViewController"/> 类型的实例。
        /// </remarks>
        public ITabViewController Controller { get; protected init; }

        /// <summary>
        /// 表示当前的视图控制器。
        /// </summary>
        /// <remarks>
        /// 该属性必须为 <see cref="Controller"/> 属性 或者 <see cref="ISplashController"/> 类型的实例。
        /// </remarks>
        public IViewController CurrentController
        {
            get => _currentController;
            set
            {
                OnControllerChanged(_currentController, value);

                SetValue(ref _currentController, value);
            }
        }
    }
}