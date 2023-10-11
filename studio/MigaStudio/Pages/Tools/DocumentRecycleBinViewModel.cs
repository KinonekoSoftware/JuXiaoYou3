using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
{
    public class DocumentRecycleBinViewModel : GalleryViewModel<DocumentCache>
    {
        private bool _isPropertyPaneOpen;

        public DocumentRecycleBinViewModel()
        {
            var dbMgr = Studio.DatabaseManager();
            DatabaseManager = dbMgr;
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();

            DeleteDocumentCommand  = AsyncCommand(DeleteDocumentImpl);
            RecoverDocumentCommand = Command(RecoverDocumentImpl);
            SelectedDocumentCommand = CommandUtilities.CreateSelectedCommand<DocumentCache>(x =>
            {
                SelectedItem       = x;
                IsPropertyPaneOpen = true;
            });
        }

        #region GalleryViewModel Override

        protected sealed override bool NeedDataSourceSynchronize()
        {
            return true;
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
            dataSource.AddMany(DocumentEngine.GetRemovedDocuments(), true);
        }

        #endregion


        private void RecoverDocumentImpl()
        {
            if (SelectedItem is null)
            {
                return;
            }

            SelectedItem.IsDeleted = false;
            SelectedItem.TimeOfModified = DateTime.Now;

            if (DocumentEngine.HasDocumentName(SelectedItem.Id, SelectedItem.Name))
            {
                this.Danger(Language.ContentDuplicatedText);
                return;
            }

            DocumentEngine.UpdateDocument(SelectedItem);
            Collection.Remove(SelectedItem);
            SelectedItem = null;
            IsPropertyPaneOpen = false;
        }

        private async Task DeleteDocumentImpl()
        {
            if (SelectedItem is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            SelectedItem.IsDeleted = false;
            IsPropertyPaneOpen = false;
            DocumentEngine.HardRemoveDocumentCache(SelectedItem);
            Collection.Remove(SelectedItem);
            SelectedItem = null;
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RecoverDocumentCommand { get; }
        public AsyncRelayCommand DeleteDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> SelectedDocumentCommand { get; }

        /// <summary>
        /// 获取或设置 <see cref="IsPropertyPaneOpen"/> 属性。
        /// </summary>
        public bool IsPropertyPaneOpen
        {
            get => _isPropertyPaneOpen;
            set { SetValue(ref _isPropertyPaneOpen, value); }
        }

        public override bool Uniqueness => true;
    }
}