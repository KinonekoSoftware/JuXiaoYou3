using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.Miga.Doc.Parts;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.Win32;
using NLog;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public class TemplateGalleryViewModel : TabViewModel, IPresentationViewModel
    {
        private const string FileNotExists = "text.FileNotFound";

        [NullCheck(UniTestLifetime.Constructor)]
        private readonly ReadOnlyObservableCollection<ModuleTemplateCache> _collection;

        [NullCheck(UniTestLifetime.Constructor)]
        private readonly BehaviorSubject<Func<ModuleTemplateCache, bool>> _sorter;

        [NullCheck(UniTestLifetime.Constructor)]
        private readonly DataPartReader _reader;

        private DocumentType        _type;
        private ModuleTemplateCache _selectedTemplate;
        private bool                _isPresentation;

        public TemplateGalleryViewModel()
        {
            TemplateEngine = Studio.DatabaseManager()
                                 .GetEngine<TemplateEngine>();

            Property = Studio.Database()
                             .Get<PresetProperty>();

            _sorter                     = new BehaviorSubject<Func<ModuleTemplateCache, bool>>(Xaml.AlwaysTrue);
            _reader                     = new DataPartReader();
            Source                      = new SourceList<ModuleTemplateCache>();
            MetadataCollection          = new ObservableCollection<MetadataCache>();
            Blocks                      = new ObservableCollection<ModuleBlock>();
            ApprovalRequired            = false;
            ManageManifestCommand       = AsyncCommand(ManageManifestImpl);
            AddTemplateCommand          = AsyncCommand(AddTemplateImpl);
            ImportTemplateCommand       = AsyncCommand(ImportTemplateImpl);
            ExportTemplateCommand       = AsyncCommand<FrameworkElement>(ExportTemplateImpl, x => x is not null && SelectedTemplate is not null);
            RemoveTemplateCommand       = AsyncCommand<ModuleTemplateCache>(RemoveTemplateImpl);
            PresentationCommand         = Command(() => IsPresentation       = true, () => SelectedTemplate is not null);
            SetSkillManifestCommand   = AsyncCommand(async () => Skill   = await PickModuleManifestImpl());
            SetCharacterManifestCommand = AsyncCommand(async () => Character = await PickModuleManifestImpl());
            SetGeographyManifestCommand = AsyncCommand(async () => Geography = await PickModuleManifestImpl());
            SetItemManifestCommand      = AsyncCommand(async () => Item      = await PickModuleManifestImpl());
            SetOtherManifestCommand     = AsyncCommand(async () => Other     = await PickModuleManifestImpl());

            Source.Connect()
                  .Filter(_sorter)
                  .ObserveOn(Scheduler)
                  .Bind(out _collection)
                  .Subscribe()
                  .DisposeWith(Collector);
        }

        protected override void OnStart()
        {
            TemplateEngine.Activate();
            Refresh();
        }


        private void Save()
        {
            Studio.Database()
                  .Upsert(Property);
        }

        private async Task AddTemplateImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ModuleFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            //
            // 
            var fileNames     = opendlg.FileNames;
            var finishedCount = 0;
            var errorCount    = 0;

            foreach (var fileName in fileNames)
            {
                if (!File.Exists(fileName))
                {
                    this.Error(string.Format(Language.GetText(FileNotExists), fileName));
                    continue;
                }

                if (!TemplateEngine.Activated)
                {
                    TemplateEngine.Activate();
                }

                var r = await TemplateEngine.AddModule(fileName);

                if (!r.IsFinished)
                {
                    errorCount++;
                }
                else
                {
                    finishedCount++;
                }
            }

            if (finishedCount == 0)
            {
                this.ErrorNotification(string.Format(Language.GetText("text.ImportModulesFailed"), finishedCount, errorCount));
            }
            else
            {
                this.SuccessfulNotification(string.Format(Language.GetText("text.ImportModulesSuccessful"), finishedCount, errorCount));
            }

            Refresh();
        }

        private async Task RemoveTemplateImpl(ModuleTemplateCache item)
        {
            if (item is null)
            {
                return;
            }

            if (!await  this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            var logger = Xaml.Get<ILogger>();
            var r      = TemplateEngine.RemoveModule(item);

            if (!r.IsFinished)
            {
                var msg = Language.GetEnum(r.Reason);

                logger.Warn(msg);
                await this.WarningNotification(msg);
            }
            else
            {
                if (item == SelectedTemplate)
                {
                    SelectedTemplate = null;
                }

                Refresh();
            }
        }

        private async Task ImportTemplateImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ModuleFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var logger        = Xaml.Get<ILogger>();
            var finishedCount = 0;
            var errorCount    = 0;

            foreach (var fileName in opendlg.FileNames)
            {
                try
                {
                    var result      = await _reader.ReadFromAsync(fileName);
                    var oldTemplate = result.Result;
                    var template    = ModuleBlockFactory.Upgrade(oldTemplate);
                    var r           = TemplateEngine.AddModule(template);

                    if (!r.IsFinished)
                    {
                        errorCount++;
                    }
                    else
                    {
                        finishedCount++;
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex.Message);
                    await this.WarningNotification(ex.Message);
                }
            }

            if (finishedCount == 0)
            {
                this.ErrorNotification(string.Format(Language.GetText("text.ImportModulesFailed"), finishedCount, errorCount));
            }
            else
            {
                this.SuccessfulNotification(string.Format(Language.GetText("text.ImportModulesSuccessful"), finishedCount, errorCount));
            }

            Refresh();
        }

        private async Task ExportTemplateImpl(FrameworkElement target)
        {
            if (target is null)
            {
                return;
            }

            var savedlg = new SaveFileDialog
            {
                FileName = SelectedTemplate.Name,
                Filter   = SubSystemString.ModuleFilter
            };

            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                var fileName = savedlg.FileName;
                var ms       = Xaml.CaptureToBuffer(target);
                var template = TemplateEngine.TemplateDB.FindById(_selectedTemplate.Id);
                var payload  = JSON.Serialize(template);


                await PNG.Write(fileName, payload, ms);
                this.SuccessfulNotification(SubSystemString.OperationOfSaveIsSuccessful);
            }
            catch (Exception ex)
            {
                this.ErrorNotification(SubSystemString.BadModule);

                Xaml.Get<ILogger>()
                    .Warn($"保存模组文件失败,文件名:{savedlg.FileName}，错误原因:{ex.Message}!");
            }
        }

        private async Task ManageManifestImpl()
        {
            await DialogService().Dialog(new ModuleManifestViewModel());
        }

        private async Task<ModulePreset> PickModuleManifestImpl()
        {
            var r = await SubSystem.Selection<ModulePreset>(
                SubSystemString.SelectTitle,
                null,
                Property.ModulePresets);

            return r.IsFinished ? r.Value : null;
        }

        public void Refresh()
        {
            //
            // 加载数据
            Source.Clear();
            Source.AddRange(TemplateEngine.TemplateCacheDB.FindAll());

            MetadataCollection.AddMany(TemplateEngine.MetadataCacheDB.FindAll(), true);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsPresentation"/> 属性。
        /// </summary>
        public bool IsPresentation
        {
            get => _isPresentation;
            set => SetValue(ref _isPresentation, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedTemplate"/> 属性。
        /// </summary>
        public ModuleTemplateCache SelectedTemplate
        {
            get => _selectedTemplate;
            set
            {
                SetValue(ref _selectedTemplate, value);

                RaiseUpdated(nameof(PresentationIntro));
                RaiseUpdated(nameof(PresentationContractList));
                RaiseUpdated(nameof(PresentationAuthorList));
                RaiseUpdated(nameof(PresentationName));

                if (_selectedTemplate is null)
                {
                    return;
                }

                var template = TemplateEngine.TemplateDB.FindById(_selectedTemplate.Id);
                if (template is null)
                {
                    return;
                }

                ExportTemplateCommand.NotifyCanExecuteChanged();
                PresentationCommand.NotifyCanExecuteChanged();
                RemoveTemplateCommand.NotifyCanExecuteChanged();
                Blocks.AddMany(template.Blocks, true);
                RaiseUpdated(nameof(Presentations));
            }
        }


        public string PresentationIntro => SubSystemString.GetIntro(SelectedTemplate?.Intro);
        public string PresentationContractList => SubSystemString.GetContractList(SelectedTemplate?.ContractList);
        public string PresentationAuthorList => SubSystemString.GetAuthor(SelectedTemplate?.AuthorList);
        public string PresentationName => SubSystemString.GetName(SelectedTemplate?.Name);

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set
            {
                SetValue(ref _type, value);
                _sorter.OnNext(_type == DocumentType.None ? Xaml.AlwaysTrue : x => x.ForType == value);
            }
        }


        public PresetProperty Property { get; }

        public ModulePreset Skill
        {
            get => Property.GetModulePreset(DocumentType.Skill);
            set
            {
                Property.SetModulePreset(DocumentType.Skill, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModulePreset Character
        {
            get => Property.GetModulePreset(DocumentType.Character);
            set
            {
                Property.SetModulePreset(DocumentType.Character, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModulePreset Geography
        {
            get => Property.GetModulePreset(DocumentType.Geography);
            set
            {
                Property.SetModulePreset(DocumentType.Geography, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModulePreset Item
        {
            get => Property.GetModulePreset(DocumentType.Item);
            set
            {
                Property.SetModulePreset(DocumentType.Item, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModulePreset Other
        {
            get => Property.GetModulePreset(DocumentType.Other);
            set
            {
                Property.SetModulePreset(DocumentType.Other, value);
                RaiseUpdated();
                Save();
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ModuleBlock> Blocks { get; }

        public IEnumerable<ModuleBlockDataUI> Presentations => Blocks.Select(ModuleBlockFactory.GetDataUI);

        [NullCheck(UniTestLifetime.Constructor)]
        public SourceList<ModuleTemplateCache> Source { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<MetadataCache> MetadataCollection { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ReadOnlyObservableCollection<ModuleTemplateCache> TemplateCollection => _collection;

        /// <summary>
        /// 
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public TemplateEngine TemplateEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand PresentationCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ManageManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModuleTemplateCache> RemoveTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<FrameworkElement> ExportTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ImportTemplateCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetSkillManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetCharacterManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetGeographyManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetItemManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetOtherManifestCommand { get; }

        public override bool Removable => false;
        public override bool Uniqueness => true;

        public RelayCommand SetSkillPresentationManifestCommand { get; }
        public RelayCommand SetCharacterPresentationManifestCommand { get; }
        public RelayCommand SetGeographyPresentationManifestCommand { get; }
        public RelayCommand SetItemPresentationManifestCommand { get; }
        public RelayCommand SetOtherPresentationManifestCommand { get; }
    }
}