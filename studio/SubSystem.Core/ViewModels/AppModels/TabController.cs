using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
// ReSharper disable StringLiteralTypo

// ReSharper disable MemberCanBeMadeStatic.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabController : ViewModelBase, ITabViewController
    {
        private ITabViewModel       _currentViewModel;
        private GlobalStudioContext _context;
        private WindowState         _windowState;

        protected TabController()
        {
            Workspace         = new ObservableCollection<ITabViewModel>();
            InactiveWorkspace = new ObservableCollection<ITabViewModel>();
            AddTabCommand     = new RelayCommand<object>(AddTabImpl);
            RemoveTabCommand  = new AsyncRelayCommand<ITabViewModel>(RemoveTabImpl);
            
        }

        public void Reset()
        {
            _currentViewModel = null;
            RaiseUpdated(nameof(CurrentViewModel));
            Workspace.ForEach(x => x.Stop());
            InactiveWorkspace.ForEach(x => x.Stop());
            Workspace.Clear();
            InactiveWorkspace.Clear();
            StartOverride();
        }

        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="windowKeyEventArgs">键盘参数</param>
        public void SetWindowEvent(WindowKeyEventArgs windowKeyEventArgs)
        {
            CurrentViewModel?.SetWindowEvent(windowKeyEventArgs);
        }

        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="windowKeyEventArgs">拖拽参数</param>
        public void SetWindowEvent(WindowDragDropArgs windowKeyEventArgs)
        {
            CurrentViewModel?.SetWindowEvent(windowKeyEventArgs);
        }

        #region AddTab / RemoveTab

        

        private void AddTabImpl(object param)
        {
            if (param is null)
            {
                RequireStartupTabViewModel();
                return;
            }

            if (param is Type vmType)
            {
                try
                {
                    var vm = Xaml.GetViewModel<ITabViewModel>(vmType);
                    Start(vm);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            if (param is ITabViewModel tab)
            {
                Start(tab);
            }
        }

        private async Task RemoveTabImpl(ITabViewModel viewModel)
        {
            if (viewModel is null)
            {
                return;
            }

            if (!viewModel.Removable)
            {
                return;
            }

            if (viewModel.ApprovalRequired && 
                !await DialogService().Danger(CloseTabItemCaption, CloseTabItemContent))
            {
                return;
            }
            
            RemoveTabWithPromise(viewModel);
        }

        internal void RemoveTabWithPromise(ITabViewModel viewModel)
        {
            var index = Workspace.IndexOf(viewModel);

            if (index > -1)
            {
                viewModel.Stop();
                Workspace.RemoveAt(index);

                //
                //
                if (Workspace.Count < MaximumWorkspaceItemCount &&
                    InactiveWorkspace.Count > 0)
                {
                    if (InactiveWorkspace.Count > 0)
                    {
                        var first = InactiveWorkspace[0];
                        InactiveWorkspace.RemoveAt(0);
                        Workspace.Add(first);
                        CurrentViewModel = first;
                    }
                    else
                    {
                        CurrentViewModel = Workspace[0];
                    }
                }

                //
                // 当关闭的是当前的页面，且标签页的数量大于0，且当前标签页不是最后一个页面
                //
                // 则显示的页面是后一个
                if (ReferenceEquals(_currentViewModel, viewModel) &&
                    Workspace.Count > 0 &&
                    index < Workspace.Count)
                {
                    CurrentViewModel = Workspace[index];
                }
                else
                {
                    //
                    // 否则选择的页面就是最后一个
                    CurrentViewModel = Workspace[^1];
                }
                //
                // 只有当只剩最后一个标签页被关闭的时候才提示创建默认页面
                if(Workspace.Count == 0)
                {
                    
                    RequireStartupTabViewModel();
                }
                
                OnRemoveViewModel(viewModel);
                return;
            }
            
            OnRemoveViewModel(viewModel);
            InactiveWorkspace.Remove(viewModel);
        }

        public Task RemoveTabItem(ITabViewModel viewModel)
        {
            return RemoveTabImpl(viewModel);
        }
        
        #endregion

        #region Start

        public sealed override void Start()
        {
            if (!IsInitialized)
            {
                StartOverride();
                IsInitialized = true;
            }
        }

        protected virtual void StartOverride()
        {
            
        }
        
        protected virtual void OnStart(RoutingEventArgs arg)
        {
            
        }

        protected sealed override void OnStartup(RoutingEventArgs arg)
        {
            _context = arg.Parameter
                          .Args[0] as GlobalStudioContext;
            OnStart(arg);
        }

        #endregion

        public override void Stop()
        {
            foreach (var page in Workspace)
            {
                page.Stop();
            }

            foreach (var page in InactiveWorkspace)
            {
                page.Stop();
            }
        }

        protected virtual void OnAddViewModel(ITabViewModel viewModel)
        {
            
        }
        
        protected virtual void OnRemoveViewModel(ITabViewModel viewModel)
        {
            
        }

        protected virtual void OnCurrentViewModelChanged(ITabViewModel oldViewModel, ITabViewModel newViewModel)
        {
            if (newViewModel is null)
            {
                return;
            }
            
            if (string.IsNullOrEmpty(newViewModel.Title))
            {
                newViewModel.Title = Language.GetTypeName(newViewModel.GetType());
            }
        }

        protected virtual void RequireStartupTabViewModel()
        {
        }

        private bool DeterminedViewModelExists(string unifiedKey)
        {
            foreach (var item in Workspace)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    CurrentViewModel = item;
                    return true;
                }
            }

            foreach (var item in InactiveWorkspace)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    if (Workspace.Count > 0)
                    {
                        var lastOneIndex = Workspace.Count - 1;
                        (Workspace[lastOneIndex], InactiveWorkspace[0]) = (InactiveWorkspace[0], Workspace[lastOneIndex]);
                        CurrentViewModel                        = Workspace[lastOneIndex];
                    }
                    else
                    {
                        CurrentViewModel = item;
                        return true;
                    }

                    break;
                }
            }

            return false;
        }
        
        private bool DeterminedViewModelExists(string unifiedKey, out ITabViewModel viewModel)
        {
            foreach (var item in Workspace)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    CurrentViewModel = item;
                    viewModel        = item;
                    return true;
                }
            }

            foreach (var item in InactiveWorkspace)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    if (Workspace.Count > 0)
                    {
                        var lastOneIndex = Workspace.Count - 1;
                        (Workspace[lastOneIndex], InactiveWorkspace[0]) = (InactiveWorkspace[0], Workspace[lastOneIndex]);
                        CurrentViewModel                        = Workspace[lastOneIndex];
                    }
                    else
                    {
                        CurrentViewModel = item;
                        viewModel        = item;
                        return true;
                    }

                    break;
                }
            }

            viewModel = null;
            return false;
        }
        
        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="viewModel">指定要打开的视图模型类型</param>
        /// <returns>返回一个新的实例。</returns>
        public TabViewModel New(Type viewModel) 
        {
            var vm = Xaml.GetViewModel<TabViewModel>(viewModel);
            vm.Startup(NavigationParameter.NewPage(vm, this));
            Start(vm);
            return vm;
        }
        
        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="parameter">指定要打开的视图模型类型</param>
        /// <returns>返回一个新的实例。</returns>
        public TabViewModel Start<TViewModel>(Parameter parameter) where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.NewPage(vm, this, parameter));
            Start(vm);
            return vm;
        }
        
        
        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="parameter">指定要打开的视图模型类型</param>
        /// <returns>返回一个新的实例。</returns>
        public TabViewModel Start<TViewModel>(string id, Parameter parameter) where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.NewPage(id, this, parameter));
            Start(vm);
            return vm;
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="cache">索引</param>
        /// <returns>返回一个新的导航参数。</returns>
        public void OpenCompose<TViewModel>(IDataCache cache)where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.OpenDocument(cache, this));
            Start(vm);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="cache">索引</param>
        /// <returns>返回一个新的导航参数。</returns>
        public void OpenDocument<TViewModel>(IDataCache cache)where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.OpenDocument(cache, this));
            Start(vm);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="viewModel">索引</param>
        /// <param name="cache">索引</param>
        /// <returns>返回一个新的导航参数。</returns>
        public void OpenDocument(Type viewModel, IDataCache cache)
        {
            var vm = Xaml.GetViewModel<TabViewModel>(viewModel);
            vm.Startup(NavigationParameter.OpenDocument(cache, this));
            Start(vm);
        }

        /// <summary>
        /// 打开视图模型（不适用ViewService）
        /// </summary>
        /// <param name="viewModel">指定要打开的视图模型类型</param>
        /// <param name="parameter">指定要打开的视图模型类型</param>
        /// <returns>返回一个新的实例。</returns>
        public TabViewModel Start(Type viewModel, Parameter parameter)
        {
            var vm = Xaml.GetViewModel<TabViewModel>(viewModel);
            vm.Startup(NavigationParameter.NewPage(vm, this, parameter));
            return vm;
        }

        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <typeparam name="TViewModel">指定要打开的视图模型类型</typeparam>
        /// <returns>返回一个新的实例。</returns>
        public TViewModel New<TViewModel>() where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.NewPage(vm, this));
            Start(vm);
            return vm;
        }

        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="id">唯一标识符</param>
        /// <typeparam name="TViewModel">指定要打开的视图模型类型</typeparam>
        /// <returns>返回一个新的实例或已经打开的视图模型。</returns>
        public TViewModel New<TViewModel>(string id) where TViewModel : TabViewModel
        {
            if (DeterminedViewModelExists(id, out var vm))
            {
                return (TViewModel)vm;
            }
            
            var vm1 = Xaml.GetViewModel<TViewModel>();
            vm1.Startup(NavigationParameter.NewPage(vm, this));
            Start(vm1);
            return vm1;
        }
        
        
        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="id">唯一标识符</param>
        /// <param name="parameter">参数</param>
        /// <typeparam name="TViewModel">指定要打开的视图模型类型</typeparam>
        /// <returns>返回一个新的实例或已经打开的视图模型。</returns>
        public TViewModel New<TViewModel>(string id, Parameter parameter) where TViewModel : TabViewModel
        {
            if (DeterminedViewModelExists(id, out var vm))
            {
                return (TViewModel)vm;
            }
            
            var vm1 = Xaml.GetViewModel<TViewModel>();
            vm1.Startup(NavigationParameter.NewPage(id, this, parameter));
            Start(vm1);
            return vm1;
        }
        
        /// <summary>
        /// 启动指定的视图模型
        /// </summary>
        /// <param name="viewModel">指定的视图模型</param>
        public void Start(ITabViewModel viewModel)
        {
            if (viewModel is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(viewModel.PageId))
            {
                viewModel.Startup(NavigationParameter.NewPage(viewModel, this));
            }

            var unifiedKey = viewModel.Uniqueness ? viewModel.GetType().FullName : viewModel.PageId;
            var result = DeterminedViewModelExists(unifiedKey);

            if (result)
            {
                return;
            }
            
            if (Workspace.Count < MaximumWorkspaceItemCount)
            {
                OnAddViewModel(viewModel);
                Workspace.Add(viewModel);
                CurrentViewModel = viewModel;
                return;
            }
                
            if(InactiveWorkspace.Count < MaximumWorkspaceItemCount)
            {
                var lastWorkspace = Workspace[^1];
                OnAddViewModel(viewModel);
                Workspace[^1] = viewModel;
                InactiveWorkspace.Insert(0, lastWorkspace);
                return;
            }

            DialogService().Notify(
                CriticalLevel.Obsoleted,
                Language.GetText("text.notify"), 
                Language.GetText("text.cannotOpenPage"));
        }

        /// <summary>
        /// 最大工作区项目数
        /// </summary>
        public int MaximumWorkspaceItemCount => (int)((SystemParameters.WorkArea.Width - 300) / 100);

        /// <summary>
        /// 当前的视图模型
        /// </summary>
        public ITabViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Suspend();
                OnCurrentViewModelChanged(_currentViewModel, value);
                SetValue(ref _currentViewModel, value);


                if (value is null)
                {
                    return;
                }

                if (value.IsInitialized)
                {
                    value.Resume();
                }
                else
                {
                    value.Start();
                }

            }
        }
        

        /// <summary>
        /// 用于绑定的<see cref="WindowState"/> 属性。
        /// </summary>
        public WindowState WindowState
        {
            get => _windowState;
            set => SetValue(ref _windowState, value);
        }
        
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// 工作区标签页
        /// </summary>
        public ObservableCollection<ITabViewModel> Workspace { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewModel> InactiveWorkspace { get; }

        /// <summary>
        /// 添加标签页
        /// </summary>
        public RelayCommand<object> AddTabCommand { get; }

        /// <summary>
        /// 移除标签页
        /// </summary>
        public AsyncRelayCommand<ITabViewModel> RemoveTabCommand { get; }


        public GlobalStudioContext Context => _context;

        internal static string CloseTabItemCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Are you sure you want to quit?",
                    CultureArea.French   => "Êtes-vous sûr de vouloir vous déconnecter?",
                    CultureArea.Japanese => "脱退は確定ですか?",
                    CultureArea.Korean   => "종료하시겠습니까?",
                    CultureArea.Russian  => "Вы уверены, что хотите уйти?",
                    _                    => "您确定要退出？"
                };
            }
        }
        
        internal static string CloseTabItemContent
        {
            get
            {
                return Language.Culture switch
                { 
                    CultureArea.English    => "The current page has not been saved, are you sure you want to exit?",
                    CultureArea.French   => "La page actuelle n’a pas été enregistrée. Êtes-vous sûr de vouloir quitter?",
                    CultureArea.Japanese => "現在のページが保存されていないので、ログアウトしますか?",
                    CultureArea.Korean   => "현재 페이지가 저장되지 않았습니다. 종료하시겠습니까?",
                    CultureArea.Russian  => "Текущая страница еще не сохранена. Вы уверены, что хотите уйти?",
                    _                    => "当前页面尚未保存，您确定要退出？"
                };
            }
        }
    }
    
    public abstract class ShellCore : TabController
    {

        protected override void StartOverride()
        {
            RequireStartupTabViewModel();
        }
    }
}