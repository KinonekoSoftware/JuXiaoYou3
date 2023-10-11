using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class GalleryViewModel<TEntity> : TabViewModel where TEntity : class
    {
        protected const int MaxItemCount     = 50;
        protected const int MaxItemCountMask = 49;
        protected const int PageCountLimited = 512;


        private int            _pageIndex;
        private TEntity        _selectedItem;
        private OrderByMethods _orderOption;
        private bool           _isFiltering;
        private string         _filterString;


        protected GalleryViewModel()
        {
            DataSource              = new List<TEntity>(128);
            Collection              = new ObservableCollection<TEntity>();
            FirstPageCommand        = Command(FirstPageImpl, CanFirstPage);
            LastPageCommand         = Command(LastPageImpl, CanLastPage);
            NextPageCommand         = Command(NextPageImpl, CanNextPage);
            PreviousPageCommand     = Command(PreviousPageImpl, CanPreviousPage);
            SearchPageCommand       = Command(SearchPage);
            SetOrderByMethodCommand = Command<OrderByMethods>(OrderPageImpl);
        }

        #region Last / Next

        private void UpdateCommands()
        {
            FirstPageCommand.NotifyCanExecuteChanged();
            LastPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
            PreviousPageCommand.NotifyCanExecuteChanged();
        }
        
        private bool CanFirstPage() => TotalPageCount > 1 && _pageIndex > 1;

        private bool CanLastPage() => TotalPageCount > 1 && _pageIndex < TotalPageCount;
        
        private bool CanNextPage() => _pageIndex + 1 <= TotalPageCount;

        private bool CanPreviousPage() => _pageIndex > 1;

        private void FirstPageImpl()
        {
            PageIndex = 1;
            PageRequest(PageIndex);
        }

        private void LastPageImpl()
        {
            
            PageIndex = TotalPageCount;
            PageRequest(PageIndex);
        }

        protected void JumpPage(int index)
        {
            if (index < 0 || index > TotalPageCount)
            {
                return;
            }

            PageIndex = index;
            PageRequest(PageIndex);
        }

        private void NextPageImpl()
        {
            PageIndex++;
            PageRequest(PageIndex);
        }

        private void PreviousPageImpl()
        {
            PageIndex--;
            PageRequest(PageIndex);
        }

        #endregion

        #region Filter / Order

        private void OrderPageImpl(OrderByMethods option)
        {
            OrderOption = option;
            PageRequest(PageIndex);
        }
        
        public void SearchPage()
        {
            IsFiltering = !string.IsNullOrEmpty(FilterString);
            PageRequest(PageIndex);
        }

        #endregion

        /// <summary>
        /// 需要同步数据源
        /// </summary>
        /// <returns>true为需要，false为不需要</returns>
        protected abstract bool NeedDataSourceSynchronize();

        /// <summary>
        /// 请求同步数据源
        /// </summary>
        /// <param name="dataSource">需要同步的数据源</param>
        protected abstract void OnRequestDataSourceSynchronize(IList<TEntity> dataSource);

        /// <summary>
        /// 计算当前的页面总数
        /// </summary>
        /// <param name="dataSource">要计算的数据源</param>
        protected abstract void OnRequestComputePageCount(IList<TEntity> dataSource);

        #region OnStart / OnResume

        protected sealed override void OnStart()
        {
            ForceInvalidateDataSource();
            StartOverride();
            base.OnStart();
        }

        protected virtual void StartOverride()
        {
            
        }

        protected override void OnInvalidateDataSource()
        {
            if (NeedDataSourceSynchronize())
            {
                ForceInvalidateDataSource();
            }
        }

        protected void ForceInvalidateDataSource()
        {
            DataSource.Clear();
            OnRequestDataSourceSynchronize(DataSource);
            OnRequestComputePageCount(DataSource);

            //
            // 重新计算
            if (PageIndex > TotalPageCount)
            {
                PageIndex = 1;
            }

            //
            //
            JumpPage(PageIndex);
        }

        #endregion

        protected abstract IList<TEntity> BuildFilteringExpression(IList<TEntity> dataSource, string keyword, OrderByMethods sorting);
        protected abstract IList<TEntity> BuildDefaultExpression(IEnumerable<TEntity> dataSource, OrderByMethods sorting);

        /// <summary>
        /// 页面跳转请求
        /// </summary>
        /// <param name="index">页面索引，从1-1024.</param>
        protected void PageRequest(int index)
        {
            IList<TEntity> unsortedDataSource;
            
            if (IsFiltering)
            {
                unsortedDataSource = BuildFilteringExpression(DataSource, FilterString, OrderOption);
                OnRequestComputePageCount(unsortedDataSource);
                index = Math.Clamp(PageIndex, 1, TotalPageCount);

            }
            else
            {
                unsortedDataSource = BuildDefaultExpression(DataSource, OrderOption);
                OnRequestComputePageCount(unsortedDataSource);
                index = Math.Clamp(index, 1, TotalPageCount);
            }
            

            if (unsortedDataSource.Count == 0)
            {
                Collection.Clear();
                return;
            }
            
            var skipElementCounts = (index - 1) * MaxItemCount;
            var minPageItemCount  = Math.Clamp(unsortedDataSource.Count - skipElementCounts, 0, MaxItemCount);

            Collection.AddMany(unsortedDataSource.Skip(skipElementCounts)
                                                 .Take(minPageItemCount), true);
            UpdateCommands();
        }

        /// <summary>
        /// 全部内容的集合
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public List<TEntity> DataSource { get; }

        /// <summary>
        /// 支持绑定的集合
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<TEntity> Collection { get; }


        /// <summary>
        /// 所有页面数量
        /// </summary>
        public int TotalPageCount { get; protected set; }

        /// <summary>
        /// 获取或设置 <see cref="FilterString"/> 属性。
        /// </summary>
        public string FilterString
        {
            get => _filterString;
            set
            {
                SetValue(ref _filterString, value);
                SearchPage();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="IsFiltering"/> 属性。
        /// </summary>
        public bool IsFiltering
        {
            get => _isFiltering;
            set => SetValue(ref _isFiltering, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="OrderOption"/> 属性。
        /// </summary>
        public OrderByMethods OrderOption
        {
            get => _orderOption;
            set => SetValue(ref _orderOption, value);
        }

        /// <summary>
        /// 选择的内容
        /// </summary>
        public TEntity SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref _selectedItem, value);
        }

        /// <summary>
        /// 当前索引
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                SetValue(ref _pageIndex, value);
                RaiseUpdated(nameof(EditablePageIndex));
            }
        }

        public int EditablePageIndex
        {
            get => PageIndex;
            set
            {
                var v = Math.Clamp(value, 1, Math.Min(TotalPageCount, PageCountLimited));
                PageIndex = v;
                JumpPage(v);
            }
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand FirstPageCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand LastPageCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand NextPageCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand PreviousPageCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SearchPageCommand { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand<OrderByMethods> SetOrderByMethodCommand { get; }
    }
}