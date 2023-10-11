using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relatives
{
    public class CharacterPedigreeViewModel : TabViewModel
    {
        private DocumentCache _selectedDocument;
        private Visibility    _relationshipPaneVisibility;
        private int           _version;

        public CharacterPedigreeViewModel()
        {
            var dbMgr = Studio.DatabaseManager();
            DatabaseManager            = dbMgr;
            DocumentEngine             = dbMgr.GetEngine<DocumentEngine>();
            Graph                      = new CharacterGraph();
            Relatives              = new ObservableCollection<CharacterRelationship>();
            RelationshipPaneVisibility = Visibility.Collapsed;
            AddDocumentCommand         = AsyncCommand(NewDocumentImpl);
            OpenDocumentCommand        = Command<DocumentCache>(OpenDocumentImpl, HasItem, true);
            ResetDocumentCommand       = Command(Reset, () => HasItem(SelectedDocument), true);
            AddRelCommand              = AsyncCommand<DocumentCache>(AddRelationshipImpl, HasItem, true);
            RemoveRelCommand           = AsyncCommand<CharacterRelationship>(RemoveRelationshipImpl);
            CaptureCommand             = AsyncCommand<FrameworkElement>(Studio.CaptureAsync, HasItem);
            _version                   = DocumentEngine.Version;
        }

        private void Initialize()
        {
            Graph.Clear();
            Graph.AddVertexRange(DocumentEngine.GetCharacterCaches());
            var rels = DocumentEngine.GetRelatives(DocumentType.Character);
            Graph.AddEdgeRange(rels);
        }

        #region OnStart / OnResume

        protected override void OnStart()
        {
            Initialize();
            base.OnStart();
        }

        protected override void OnResume()
        {
            if (_version != DocumentEngine.Version)
            {
                Initialize();
                _version = DocumentEngine.Version;
            }
        }

        #endregion

        #region Document: Add / Remove / Reset
        

        private async Task NewDocumentImpl()
        {
            await DocumentUtilities.AddDocument(DocumentEngine, DocumentType.Character, x =>
            {
                Graph.AddVertex(x);
            });
        }
        
        private void OpenDocumentImpl(DocumentCache cache)
        {
            DocumentUtilities.OpenDocument(Controller, cache);
        }

        private void Reset()
        {
            SelectedDocument = null;
        }
        
        #endregion

        #region Relationship: Add / Remove

        private async Task AddRelationshipImpl(DocumentCache source)
        {
            var hash = Relatives.Flat(x=> x.Source.Id, x => x.Target.Id)
                                    .ToHashSet();
            hash.Add(source.Id);
            var r = await SubSystem.Select(DocumentEngine.GetDocuments(DocumentType.Character)
                                                                       .Where(x => !hash.Contains(x.Id)));

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await NewRelativeViewModel.New(source, r.Value, DocumentType.Character);
            
            if (!r1.IsFinished)
            {
                return;
            }

            DocumentEngine.AddRelationship(r1.Value);
            Graph.AddEdge(r1.Value);
            
            if (SelectedDocument.Id == source.Id)
            {
                Relatives.Add(r1.Value);
            }
        }

        private async Task RemoveRelationshipImpl(CharacterRelationship rel)
        {
            if (rel is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            

            DocumentEngine.RemoveRelationship(rel.Id);
            Relatives.Remove(rel);
            Graph.RemoveEdge(rel);
        }
        
        #endregion

        /// <summary>
        /// 人物关系面板的可视性
        /// </summary>
        public Visibility RelationshipPaneVisibility
        {
            get => _relationshipPaneVisibility;
            set => SetValue(ref _relationshipPaneVisibility, value);
        }

        /// <summary>
        /// 选择的文档
        /// </summary>
        public DocumentCache SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                SetValue(ref _selectedDocument, value);
                RelationshipPaneVisibility = value is not null ? Visibility.Visible : Visibility.Collapsed;
                Relatives.Clear();
                if (_selectedDocument is null)
                {
                    return;
                }

                Graph.TryGetInEdges(value, out var edgeA);
                Graph.TryGetOutEdges(value, out var edgeB);
                Relatives.AddMany(edgeA);
                Relatives.AddMany(edgeB);
            }
        }

        /// <summary>
        /// 人物关系图
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public CharacterGraph Graph { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        /// <summary>
        /// 当前的所有人物关系
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<CharacterRelationship> Relatives { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> OpenDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand ResetDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> AddRelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<CharacterRelationship> RemoveRelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<FrameworkElement> CaptureCommand { get; }
    }
}