using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{
    public class DocumentGalleryViewModelEx : TabViewModel
    {
        private const int MaxCountPerPage     = 40;
        private const int MaxCountPerPageMask = 39;
        private const int MaxPageCount        = 250;
        private const int MinPageIndex        = 1;


        private readonly ReadOnlyObservableCollection<IDataCache> _collection;

        public DocumentGalleryViewModelEx()
        {
            var dbMgr = Studio.DatabaseManager();
            DatabaseManager = dbMgr;
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            TemplateEngine  = dbMgr.GetEngine<TemplateEngine>();
            DocumentSource  = new SourceList<IDataCache>();

            NewDocumentCommand         = AsyncCommand(NewDocumentImpl);
            SelectDocumentCommand      = Command<DocumentCache>(SelectDocumentImpl);
            ChangeAvatarCommand        = AsyncCommand<DocumentCache>(ChangeDocumentAvatarImpl, HasDocument);
            GotoTemplateGalleryCommand = AsyncCommand(GotoTemplateGalleryImpl);
            NextPageCommand            = Command(NextPageImpl, CanNextPage);
            LastPageCommand            = Command(LastPageImpl, CanLastPage);


            DocumentSource.Connect()
                          .ObserveOn(Scheduler)
                          .Bind(out _collection)
                          .Subscribe()
                          .DisposeWith(Collector);

            //
            // 初始化
            PageIndex = MinPageIndex;
        }

        #region Command

        #region Last / Next

        private bool CanNextPage() => _pageIndex + 1 < _totalPageCount;

        private bool CanLastPage() => _pageIndex > 1;

        private void NextPageImpl()
        {
            PageIndex++;
            LoadPage();
        }

        private void LastPageImpl()
        {
            PageIndex--;
            LoadPage();
        }

        #endregion
        
        #region Document

        private static bool HasDocument(IDataCache cache) => cache is not null;

        private async Task NewDocumentImpl()
        {
            
        }


        private void SelectDocumentImpl(DocumentCache index)
        {
            if (index is null)
            {
                return;
            }

            Selected = index;

            // TODO: 进一步操作
            IsDocumentPropertyPaneOpen = Selected is not null;
        }


        private async Task ChangeDocumentAvatarImpl(DocumentCache index)
        {
            if (index is null)
            {
                return;
            }

            var r = await ImageUtilities.Avatar();

            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (ImageEngine.HasFile(md5))
            {
                var fr = ImageEngine.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = ImageUtilities.GetAvatarName();
                buffer.Seek(0, SeekOrigin.Begin);
                ImageEngine.WriteAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image
                };

                ImageEngine.AddFile(record);
            }

            index.Avatar = avatar;
            DocumentEngine.UpdateDocument(index);
        }

        #endregion

        #region GotoPage

        private async Task GotoTemplateGalleryImpl()
        {
           
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 加载图片，确保图片的索引位于（1，250），也就是最大支持7500个设定
        /// </summary>
        /// <remarks>
        /// 注意：在加载页面之前，必须使用Update方法更新总页面数。
        /// </remarks>
        private void LoadPage()
        {
            var maxPageCount = Math.Clamp(_totalPageCount, 1, MaxPageCount);
            // 值范围：[1,250]
            var index = Math.Clamp(
                _pageIndex, 
                MinPageIndex,
                maxPageCount);
            
            // Take(MaxCountPerPage)可能会出错
            // 需要Take(Math.Min(,1,MaxCountPerPage)
            var takeElementCount =  _totalElementCount > MaxCountPerPage ? MaxCountPerPage : _totalElementCount;
            
            var iterator = DocumentEngine.GetCaches()
                                         .Skip((index - 1) * MaxCountPerPage)
                                         .Take(takeElementCount);

            // 不进行空检查，因为这个错误在发生的时候会导致整个应用无法正常运行
            DocumentSource.Clear();
            DocumentSource.AddRange(iterator);
        }

        private void Update()
        {
            _totalElementCount = DocumentEngine.GetCacheCount();
            _totalPageCount    = (_totalElementCount + MaxCountPerPageMask) / MaxCountPerPage;
        }



        #endregion

        #region OnStart / Refresh / Resume

        public void Refresh()
        {
            if (!DocumentEngine.Activated)
            {
                // 在第一次加载文档引擎的时候，各项依赖属性都未刷新
                // 需要调用Update方法，更新属性
                DocumentEngine.Activate();
                Update();
            }
            else if (Version == DocumentEngine.Version)
            {
                //
                // 如果版本号相同就不需要更新，避免频繁的创建对象
                return;
            }

            //
            // 每次调用该方法，都需要同步引擎的版本和当前的版本。
            Version = DocumentEngine.Version;

            //
            // 加载内容
            LoadPage();
        }

        protected override void OnStart()
        {
            Refresh();
            base.OnStart();
        }

        protected override void OnInvalidateDataSource()
        {
            Refresh();
        }

        #endregion

        #region Properties

        #region Bindable Properties

        private int        _pageIndex;
        private int        _totalPageCount;
        private int        _totalElementCount;
        private IDataCache _selected;
        private bool       _isDocumentPropertyPaneOpen;

        /// <summary>
        /// 获取或设置 <see cref="IsDocumentPropertyPaneOpen"/> 属性。
        /// </summary>
        public bool IsDocumentPropertyPaneOpen
        {
            get => _isDocumentPropertyPaneOpen;
            set => SetValue(ref _isDocumentPropertyPaneOpen, value);
        }


        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public IDataCache Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TotalPage"/> 属性。
        /// </summary>
        public int TotalPage
        {
            get => _totalPageCount;
            set
            {
                SetValue(ref _totalPageCount, value);
                LastPageCommand.NotifyCanExecuteChanged();
                NextPageCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="PageIndex"/> 属性。
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                SetValue(ref _pageIndex, value);
                LastPageCommand.NotifyCanExecuteChanged();
                NextPageCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion

        [NullCheck(UniTestLifetime.Constructor)]
        public SourceList<IDataCache> DocumentSource { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public ReadOnlyObservableCollection<IDataCache> Collection => _collection;
        
        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public TemplateEngine TemplateEngine { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        #region Commands

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand NextPageCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand LastPageCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> SelectDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> ChangeAvatarCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand GotoTemplateGalleryCommand { get; }
        
        #endregion

        #endregion
        
        public override bool Uniqueness => true;
    }
}