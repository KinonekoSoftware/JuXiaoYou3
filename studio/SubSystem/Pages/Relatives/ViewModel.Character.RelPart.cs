using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;
using Acorisoft.FutureGL.MigaStudio.Pages.Relatives;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class CharacterRelPartViewModel
    {
        private DocumentCache _selectedDocument;
        private Visibility    _relationshipPaneVisibility;
        private int           _version;
        private bool          _isHidden;

        public CharacterRelPartViewModel(DocumentEditorBase owner, PartOfRel rel)
        {
            Owner  = owner;
            Detail = rel;
            var dbMgr = Studio.DatabaseManager();
            DatabaseManager            = dbMgr;
            DocumentEngine             = dbMgr.GetEngine<DocumentEngine>();
            Graph                      = new CharacterGraph();
            Relatives                  = new ObservableCollection<CharacterRelationship>();
            RelationshipPaneVisibility = Visibility.Collapsed;
            AddRelCommand              = AsyncCommand<DocumentCache>(AddRelationshipImpl);
            EditRelCommand             = AsyncCommand<CharacterRelationship>(EditRelationshipImpl);
            CaptureCommand             = AsyncCommand<FrameworkElement>(Studio.CaptureAsync, HasItem);
            RemoveRelCommand           = AsyncCommand<CharacterRelationship>(RemoveRelationshipImpl);
            _version                   = DocumentEngine.Version;
        }

        private void Initialize()
        {
            var cache = Owner.Cache;
            var rels = DocumentEngine.GetRelatives(DocumentType.Character, cache.Id)
                                     .ToArray();
            var hash = rels.Flat(x => x.Source.Id, x => x.Target.Id)
                           .ToHashSet();
            Graph.Clear();
            Graph.AddVertex(cache);
            Graph.AddVertexRange(DocumentEngine.GetCaches(hash));
            Graph.AddEdgeRange(rels);
            _selectedDocument = cache;
        }

        #region OnStart / OnResume

        public sealed override void Start()
        {
            Initialize();
        }

        public override void Resume()
        {
            if (_version != DocumentEngine.Version)
            {
                Initialize();
                _version = DocumentEngine.Version;
            }
        }

        #endregion


        #region Relationship: Add / Edit / Remove

        private async Task AddRelationshipImpl(DocumentCache source)
        {
            var hash = Relatives.Flat(x => x.Source.Id, x => x.Target.Id)
                                .ToHashSet();
            hash.Add(source.Id);

            //var r = await SubSystem.Select(DocumentType.Character,hash);
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
            Graph.AddVertex(r.Value);
            Graph.AddEdge(r1.Value);

            if (SelectedDocument.Id == source.Id)
            {
                Relatives.Add(r1.Value);
            }
        }

        private async Task EditRelationshipImpl(CharacterRelationship source)
        {
            var r1 = await NewRelativeViewModel.Edit(source.Source, source.Target, source);

            if (!r1.IsFinished)
            {
                return;
            }

            if (source.IsSwitch)
            {
                source.Reset();
                DocumentEngine.EditRelationship(r1.Value);
                source.Switch();
            }
            else
            {
                DocumentEngine.EditRelationship(r1.Value);
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
            Graph.RemoveVertex(rel.Target);
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

                IsHidden = false;
                
                // 别人和自己的关系
                Graph.TryGetInEdges(value, out var edgeA);
                
                // 自己和别人的关系
                Graph.TryGetOutEdges(value, out var edgeB);
                Relatives.AddMany(edgeA.Select(x => x.Source.Id == SelectedDocument.Id ? x : x.Switch()));
                Relatives.AddMany(edgeB.Select(x => x.Target.Id == SelectedDocument.Id ? x : x.Switch()));
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
        public AsyncRelayCommand<DocumentCache> AddRelCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<CharacterRelationship> EditRelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<CharacterRelationship> RemoveRelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<FrameworkElement> CaptureCommand { get; }

        /// <summary>
        /// 编辑器
        /// </summary>
        public DocumentEditorBase Owner { get; }

        /// <summary>
        /// 
        /// </summary>
        public PartOfRel Detail { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                SetValue(ref _isHidden, value);
                _relationshipPaneVisibility = value ? Visibility.Collapsed : Visibility.Visible;
                RaiseUpdated(nameof(RelationshipPaneVisibility));
            }
        }
    }
}