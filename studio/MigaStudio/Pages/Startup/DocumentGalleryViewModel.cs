using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class EasyDocumentGalleryViewModelProxy : BindingProxy<DocumentGalleryViewModel>
    {
    }

    public partial class DocumentGalleryViewModel : GalleryViewModel<DocumentCache>
    {
        private int  _version;
        private bool _isPropertyPaneOpen;

        public DocumentGalleryViewModel()
        {
            var dbMgr = Studio.DatabaseManager();
            DatabaseManager = dbMgr;
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            KeywordEngine   = dbMgr.GetEngine<KeywordEngine>();
            Keywords        = new ObservableCollection<Keyword>();
            Root            = new ObservableCollection<DirectoryRootUI>();
            Directories     = new ObservableCollection<DirectorySupportUI>();
            _version        = DocumentEngine.Version;

            SelectedDocumentCommand = CommandUtilities.CreateSelectedCommand<DocumentCache>(x =>
            {
                Keywords.AddMany(KeywordEngine.GetKeywords(x.Id), true);
                SelectedItem       = x;
                IsPropertyPaneOpen = true;
            });
            
            //
            // Dir
            OpenDirectoryPaneCommand  = Command(OpenDirectoryPaneImpl);
            ClearDirectoryModeCommand = Command(ClearDirectoryModeImpl);

            OpenInNewTabCommand = Command(OpenInNewTabImpl);
            
            NewDocumentCommand          = AsyncCommand(NewDocumentImpl);
            LockOrUnlockDocumentCommand = Command<DocumentCache>(LockOrUnlockDocumentImpl);
            ChangeDocumentCommand       = AsyncCommand<DocumentCache>(ChangeDocumentImpl);
            RemoveDocumentCommand       = AsyncCommand<DocumentCache>(RemoveDocumentImpl);
            OpenDocumentCommand         = Command<DocumentCache>(OpenDocumentImpl);
            AddKeywordCommand           = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand        = AsyncCommand<Keyword>(RemoveKeywordImpl, x => x is not null);
        }

        private void OpenInNewTabImpl()
        {
            Controller.Start(this);
        }

        #region GalleryViewModel Override

        protected sealed override bool NeedDataSourceSynchronize()
        {
            if (_version != DocumentEngine.Version)
            {
                _version = DocumentEngine.Version;
                return true;
            }

            return false;
        }

        protected override void OnRequestComputePageCount(IList<DocumentCache> dataSource)
        {
            var count = dataSource.Count;
            TotalPageCount = ((count == 0 ? 1 : count) + MaxItemCountMask) / MaxItemCount;

            if (TotalPageCount == 1)
            {
                PageIndex = 1;
            }
            else
            {
                PageIndex = Math.Clamp(
                    PageIndex,
                    1,
                    Math.Min(TotalPageCount, PageCountLimited));
            }
        }

        protected override IList<DocumentCache> BuildDefaultExpression(IEnumerable<DocumentCache> dataSource, OrderByMethods sorting)
        {
            if (sorting == OrderByMethods.AscendingByName)
            {
                return dataSource.OrderBy(x => x.Name)
                                 .ToList();
            }

            if (sorting == OrderByMethods.DescendingByName)
            {
                return dataSource.OrderByDescending(x => x.Name)
                                 .ToList();
            }

            if (sorting == OrderByMethods.AscendingByTimeOfCreated)
            {
                return dataSource.OrderBy(x => x.TimeOfCreated)
                                 .ToList();
            }

            if (sorting == OrderByMethods.DescendingByTimeOfCreated)
            {
                return dataSource.OrderByDescending(x => x.TimeOfCreated)
                                 .ToList();
            }

            if (sorting == OrderByMethods.AscendingByTimeOfModified)
            {
                return dataSource.OrderBy(x => x.TimeOfModified)
                                 .ToList();
            }

            return dataSource.OrderByDescending(x => x.TimeOfModified)
                             .ToList();
        }


        protected override IList<DocumentCache> BuildFilteringExpression(IList<DocumentCache> dataSource, string keyword, OrderByMethods sorting)
        {
            var filteredSource = dataSource.Where(x => x.Name.Contains(keyword));
            return BuildDefaultExpression(filteredSource, sorting);
        }

        protected sealed override void OnRequestDataSourceSynchronize(IList<DocumentCache> dataSource)
        {
            dataSource.AddMany(DocumentEngine.GetDocuments(Type));
        }

        #endregion

        #region OnStart / OnResume

        protected sealed override void OnStart(Parameter parameter)
        {
            Type = (DocumentType)parameter.Args[0];
            Initialize();
            Title = Language.GetEnum(Type);
            base.OnStart(parameter);
        }

        protected override void OnResume()
        {
            if (SelectedItem is not null)
            {
                SelectedItem = DocumentUtilities.UpdateDocument(
                    DocumentEngine,
                    SelectedItem.Id,
                    DataSource,
                    Collection);

                Keywords.AddMany(KeywordEngine.GetKeywords(SelectedItem.Id), true);
            }

            InitializeWhenResume();
        }

        #endregion

        private async Task NewDocumentImpl()
        {
            await DocumentUtilities.AddDocument(DocumentEngine, Type, async x =>
            {
                if (SelectedDirectory is not null)
                {
                    await KeywordUtilities.ForceAddKeyword(
                        x.Id, 
                        new Keyword
                        {
                            Id         = ID.Get(),
                            Name       = SelectedDirectory.Name,
                            DocumentId = x.Id
                        },
                        KeywordEngine,
                        SetDirtyState,
                        this.WarningNotification);
                }

                DataSource.Add(x);
                Collection.Add(x);
            });
        }


        private async Task ChangeDocumentImpl(DocumentCache cache)
        {
            await DocumentUtilities.ChangeDocument(DocumentEngine, ImageEngine, cache, _ => { this.SuccessfulNotification(SubSystemString.OperationOfSaveIsSuccessful); });
        }

        private void LockOrUnlockDocumentImpl(DocumentCache cache)
        {
            DocumentUtilities.LockOrUnlock(DocumentEngine, cache);
        }

        private void OpenDocumentImpl(DocumentCache cache)
        {
            DocumentUtilities.OpenDocument(Controller, cache);
        }

        private async Task RemoveDocumentImpl(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (cache.IsDeleted)
            {
                return;
            }

            if (cache.IsLocked)
            {
                await this.WarningNotification(SubSystemString.CannotDelete);
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            DocumentUtilities.RemoveDocument(KeywordEngine, DocumentEngine, cache, x =>
            {
                if (ReferenceEquals(cache, SelectedItem))
                {
                    SelectedItem       = null;
                    IsPropertyPaneOpen = false;
                }
                
                DataSource.Remove(x);
                Collection.Remove(x);
                this.SuccessfulNotification(SubSystemString.OperationOfRemoveIsSuccessful);
            });
        }

        private async Task AddKeywordImpl()
        {
            await KeywordUtilities.AddKeyword(
                SelectedItem.Id,
                Keywords,
                KeywordEngine,
                SetDirtyState,
                this.WarningNotification);
        }

        private async Task RemoveKeywordImpl(Keyword item)
        {
            await KeywordUtilities.RemoveKeyword(
                item,
                Keywords,
                KeywordEngine,
                SetDirtyState,
                this.Error);
        }

        public ObservableCollection<Keyword> Keywords { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Keyword> RemoveKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public KeywordEngine KeywordEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> SelectedDocumentCommand { get; }
        public RelayCommand OpenInNewTabCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> ChangeDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> LockOrUnlockDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> OpenDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> RemoveDocumentCommand { get; }

        public DocumentType Type { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="IsPropertyPaneOpen"/> 属性。
        /// </summary>
        public bool IsPropertyPaneOpen
        {
            get => _isPropertyPaneOpen;
            set
            {
                SetValue(ref _isPropertyPaneOpen, value);
            }
        }
    }
}