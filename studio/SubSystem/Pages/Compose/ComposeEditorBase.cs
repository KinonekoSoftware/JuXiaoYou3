using System.Reactive.Linq;
using System.Windows.Input;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Editors;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Editors.Models;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public abstract partial class ComposeEditorBase : DataPartEditable<ComposeCache, Compose>, IComposeEditor
    {
        protected ComposeEditorBase()
        {
            var dbMgr = Studio.DatabaseManager();
            WorkspaceMapping            = new Dictionary<Type, IWorkspace>();
            InternalWorkspaceCollection = new ObservableCollection<IWorkspace>();
            Outlines                    = new ObservableCollection<IOutlineModel>();
            ReferenceDocuments          = new ObservableCollection<DocumentCache>();
            WorkspaceCollection         = new ReadOnlyObservableCollection<IWorkspace>(InternalWorkspaceCollection);
            DataParts                   = new ObservableCollection<DataPart>();
            Services                    = new ObservableCollection<ComposeService>();
            Xaml.Get<IAutoSaveService>()
                .Observable
                .ObserveOn(Scheduler)
                .Subscribe(_ => { Save(); })
                .DisposeWith(Collector);
            DatabaseManager = dbMgr;
            ComposeEngine   = dbMgr.GetEngine<ComposeEngine>();
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();

            
            OpenDocumentCommand = Command<DocumentCache>(OpenDocumentImpl);
            SaveComposeCommand  = Command(Save);
            NewComposeCommand   = AsyncCommand(async () => await ComposeUtilities.AddComposeAsync(ComposeEngine));
            UndoCommand         = Command(UndoImpl);
            RedoCommand         = Command(RedoImpl);


            Initialize();
        }

        private void Save()
        {
            ComposeEngine.UpdateCompose(Document, Cache);
            SetDirtyState(false);
        }

        private void SaveWithNotification()
        {
            Save();
            this.SuccessfulNotification(SubSystemString.OperationOfSaveIsSuccessful);
        }

        private void Initialize()
        {
            // CreateSubViews(InternalSubViews);
            // SelectedSubView = InternalSubViews.FirstOrDefault();

            //
            // 添加绑定
            AddKeyBinding(ModifierKeys.Control, Key.S, SaveWithNotification);
        }

        private void ActivateAllEngines()
        {
            var engines = new DataEngine[]
            {
                ImageEngine,
                ComposeEngine,
                KeywordEngine,
            };

            foreach (var engine in engines)
            {
                if (!engine.Activated)
                {
                    engine.Activate();
                }
            }
        }
        
        protected override void OnDirtyStateChanged(bool state)
        {
            SetTitle(Document.Name, state);
        }
        
        protected override void OpeningDocument(ComposeCache cache, Compose document)
        {
            SynchronizeKeywords();
        }

        #region OnLoad

        protected override bool OnDataPartAddingBefore(DataPart part)
        {
            if (part is PartOfRtf rtf)
            {
                DataPartTrackerOfType.Remove(part.GetType());
                Document.Parts.Remove(rtf);
                return true;
            }
            
            return false;
        }

        protected override void OnDataPartAddingAfter(DataPart part)
        {
            if (DataPartTrackerOfType.TryAdd(part.GetType(), part))
            {
                if (part is PartOfMarkdown pom)
                {
                    CreateWorkspace(pom);
                }

                if (part is PartOfRtf por)
                {
                    //
                    
                }
                else if (part is PartOfAlbum poa)
                {
                    Album = poa;
                    Services.Add(ServiceViewContainer.GetService(poa));
                }
                else if (part is PartOfManifest pom2)
                {
                    DataParts.Add(part);
                }
            }
        }

        #endregion

        protected override void OnResume()
        {
            Xaml.Get<ITokenizerService>()
                .Invalidate();
            base.OnResume();
        }

        protected override void ReleaseManagedResourcesOverride()
        {
            ReleaseWorkspace();
            base.ReleaseManagedResourcesOverride();
        }
        
        
        public string Name
        {
            get => Cache.Name;
            set
            {
                Cache.Name       = value;
                Document.Name    = value;
                ApprovalRequired = true;
                RaiseUpdated();
            }
        }

        private IOutlineModel _selectedOutline;

        /// <summary>
        /// 获取或设置 <see cref="SelectedOutline"/> 属性。
        /// </summary>
        public IOutlineModel SelectedOutline
        {
            get => _selectedOutline;
            set
            {
                SetValue(ref _selectedOutline, value);
                Workspace.ScrollTo(value);
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<DataPart> DataParts { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<IOutlineModel> Outlines { get; }
        
        public ObservableCollection<DocumentCache> ReferenceDocuments { get; }


        [NullCheck(UniTestLifetime.Startup)]
        public ObservableCollection<ComposeService> Services { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ComposeEngine ComposeEngine { get; }

        public PartOfAlbum Album { get; private set; }
    }
}