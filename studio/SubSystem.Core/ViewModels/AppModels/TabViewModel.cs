using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Inputs;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    [DebuggerDisplay("{PageId}-{Title}")]
    public abstract class TabViewModel : PageViewModel, IEquatable<TabViewModel>, ITabViewModel
    {
        private const string Unsave = "● {0}";
        private       string _title;
        private       bool   _isPinned;
        private       bool   _dirtyState;


        protected TabViewModel()
        {
            ApprovalRequired = false;
            KeyInputs        = new List<KeyInput>(8);
        }


        protected void SetTitle(string title, bool useUnsavePattern = false)
        {
            Title = useUnsavePattern ? string.Format(Unsave, title) : title;
        }
        
        #region SetDirtyState

        protected virtual void OnDirtyStateChanged(bool state)
        {
            
        }

        public void SetDirtyState(bool state = true)
        {
            _dirtyState = state;
            OnDirtyStateChanged(state);
            ApprovalRequired = state;
        }

        #endregion

        #region Override

        public bool Equals(TabViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Uniqueness ? other.GetType() == GetType() : PageId == other.PageId;
        }

        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TabViewModel)obj);
        }

        public override int GetHashCode()
        {
            return GetHashCodeIntern();
        }
        
        protected int GetHashCodeIntern() => string.IsNullOrEmpty(PageId) ? base.GetHashCode() : PageId.GetHashCode();

        
        public override string ToString()
        {
            return string.IsNullOrEmpty(Title) ? PageId : Title;
        }
        #endregion

        #region OnStartup

        

        /// <summary>
        /// 传递参数。
        /// </summary>
        /// <param name="arg"></param>
        protected sealed override void OnStartup(RoutingEventArgs arg)
        {
            PageId     = Uniqueness ? GetUniquenessKey() : arg.Id;
            Controller = (TabController)arg.Controller;
            OnStart(arg.Parameter ?? Parameter.Empty);
        }

        private string GetUniquenessKey()
        {
            return $"{GetHashCode()}-{GetType().FullName}";  
        }

        #endregion

        #region Resume / OnResume

        public sealed override void Resume()
        {
            base.Resume();
            OnInvalidateDataSource();
            OnResume();
        }

        protected virtual void OnInvalidateDataSource(){}

        protected virtual void OnResume()
        {
            
        }

        #endregion

        #region Controller

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public TViewModel New<TViewModel>() where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.NewPage(vm, Controller));
            Controller.Start(vm);
            return vm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <typeparam name="TViewModel"></typeparam>
        public void New<TViewModel>(IDataCache cache) where TViewModel : TabViewModel
        {
            var vm = Xaml.Get<TViewModel>();
            vm.Startup(NavigationParameter.OpenDocument(cache, Controller));
            Controller.Start(vm);
        }

        #endregion

        #region WindowEventArgs

        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="e">键盘参数</param>
        public void SetWindowEvent(WindowKeyEventArgs e)
        {
            OnKeyDown(e);
        }

        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="e">拖拽参数</param>
        public void SetWindowEvent(WindowDragDropArgs e)
        {
            OnDragDrop(e);
        }

        #endregion

        public sealed override void Stop()
        {
            IsInitialized = false;
            OnStop();
            base.Stop();
        }

        protected virtual void OnStop()
        {
            
        }

        protected virtual void OnDragDrop(WindowDragDropArgs e)
        {
            
        }

        #region KeyboardBinding

        private void OnKeyDown(WindowKeyEventArgs e)
        {
            if (KeyInputs.Count == 0 || e.IsDown)
            {
                return;
            }

            var modifier = e.Args.KeyboardDevice.Modifiers;
            var key      = e.Args.Key;

            foreach (var input in KeyInputs)
            {
                if (input.Modifiers == ModifierKeys.None && key == input.Key)
                {
                    input.Expression?.Invoke();
                }
                else if (key == input.Key && modifier == input.Modifiers)
                {
                    input.Expression?.Invoke();
                }
            }
        }

        protected void AddKeyBinding(Key key, Action expression)
        {
            if (expression is null)
            {
                return;
            }

            KeyInputs.Add(new KeyInput
            {
                Key        = key,
                Modifiers  = ModifierKeys.None,
                Expression = expression
            });
        }

        protected void AddKeyBinding(ModifierKeys modifier, Key key, Action expression)
        {
            if (expression is null)
            {
                return;
            }

            KeyInputs.Add(new KeyInput
            {
                Key        = key,
                Modifiers  = modifier,
                Expression = expression
            });
        }

        protected List<KeyInput> KeyInputs { get; }

        #endregion

        /// <summary>
        /// 是否未保存？
        /// </summary>
        public bool DirtyState => _dirtyState;

        protected TabController Controller { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        /// <summary>
        /// 用于表示当前的视图模型的唯一标识符。
        /// </summary>
        public string PageId { get; protected set; }


        /// <summary>
        /// 是否固定
        /// </summary>
        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                if (_isPinned != value)
                {
                    SetValue(ref _isPinned, value);
                }
            }
        }
        
        public int Version { get; protected set; }

        /// <summary>
        /// 是否可被关闭
        /// </summary>
        public virtual bool Removable => true;

        /// <summary>
        /// 需要询问
        /// </summary>
        public bool ApprovalRequired { get; protected set; }

        /// <summary>
        /// 用来表示当前的视图模型是否为唯一的。
        /// </summary>
        /// <remarks>
        /// <see cref="Uniqueness"/> 属性用来表示是否唯一，这个唯一是按照类型来算的。如果这个值为true，那么只能存在一个打开的类型。
        /// </remarks>
        public virtual bool Uniqueness => false;
    }

    public abstract class TabStartupViewModel : TabViewModel
    {
        public sealed override bool Removable => false;
        public sealed override bool Uniqueness => true;
    }
}