using System.Linq;
using System.Windows;
using System.Windows.Input;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using NLog;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public class TemplateEditorViewModel : TabViewModel, IPresentationViewModel
    {
        [NullCheck(UniTestLifetime.Constructor)]
        private readonly HashSet<string> _metaHashSet;

        private string            _name;
        private string            _authorList;
        private string            _contractList;
        private string            _organizations;
        private string            _intro;
        private DocumentType      _forType;
        private string            _for;
        private string            _id;
        private ModuleBlockEditUI _selectedBlock;
        private bool              _isPresentationPaneOpen;
        private bool              _dirty;
        private int               _templateVersion;

        public TemplateEditorViewModel()
        {
            _metaHashSet                = new HashSet<string>();
            Id                          = ID.Get();
            TemplateVersion                     = 1;
            ForType                     = DocumentType.Character;
            Blocks                      = new ObservableCollection<ModuleBlockEditUI>();
            MetadataList                = new ObservableCollection<MetadataCache>();
            OpenPresentationPaneCommand = new RelayCommand(() => IsPresentationPaneOpen = true);

            NewTemplateCommand         = Command(NewTemplateImpl);
            OpenTemplateCommand        = AsyncCommand(OpenTemplateImpl);
            SaveTemplateCommand        = AsyncCommand(SaveTemplateImpl);
            NewBlockCommand            = AsyncCommand(NewBlockImpl);
            EditBlockCommand           = AsyncCommand<ModuleBlockEditUI>(EditBlockImpl, HasElement);
            RemoveBlockCommand         = AsyncCommand<ModuleBlockEditUI>(RemoveBlockImpl, HasElement);
            ShiftUpBlockCommand        = Command<ModuleBlockEditUI>(ShiftUpBlockImpl, HasElement);
            ShiftDownBlockCommand      = Command<ModuleBlockEditUI>(ShiftDownBlockImpl, HasElement);
            RemoveAllBlockCommand      = AsyncCommand(RemoveAllBlockImpl);
            RefreshMetadataListCommand = Command(RefreshMetadataListImpl);

            SetDirtyState(false);

            AddKeyBinding(ModifierKeys.Control, Key.N, KeyboardInput_New);
            AddKeyBinding(ModifierKeys.Control, Key.O, KeyboardInput_Open);
            AddKeyBinding(ModifierKeys.Control, Key.S, KeyboardInput_Save);
        }

        #region Keyboard Input

        private void KeyboardInput_New()
        {
            NewTemplateImpl();
        }

        private async void KeyboardInput_Save()
        {
            await SaveTemplateImpl();
        }

        private async void KeyboardInput_Open()
        {
            await OpenTemplateImpl();
        }

        #endregion

        private static bool HasElement<T>(T element) where T : class => element is not null;

        
        protected override void OnDirtyStateChanged(bool state)
        {
            var name = string.IsNullOrEmpty(Name) ? Id : Name;
            _dirty = state;
            SetTitle(name, _dirty);
        }

        #region Templates

        private void NewTemplateImpl()
        {
            // 新建模板的行为是:
            // 1) 直接弹出新的标签页
            Controller.New<TemplateEditorViewModel>();
        }

        // private ModuleTemplate OpenOrUpgrade(string payload)
        // {
        //     try
        //     {
        //
        //     }
        //     catch()
        //     {
        //         
        //     }
        // }


        private async Task OpenTemplateImpl()
        {
            // 打开模板的行为是:
            // 1) 判断当前的页面是否保存
            // 2) 选择文件
            // 3) 打开文件并赋值

            // 行为 1)
            if (_dirty)
            {
                var r = await this.Warning(SubSystemString.AreYouSureCreateNew);

                if (!r) return;
            }

            var opendlg = Studio.Open(SubSystemString.ModuleFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                // 3) 打开文件并赋值
                var fileName        = opendlg.FileName;
                var templatePayload = await PNG.ReadDataAsync(fileName);
                var template        = JSON.FromJson<ModuleTemplate>(templatePayload);

                Id              = template.Id;
                AuthorList      = template.AuthorList;
                TemplateVersion = template.Version;
                Name            = template.Name;
                Intro           = template.Intro;
                Organizations   = template.Organizations;
                ContractList    = template.ContractList;
                For             = template.For;
                ForType         = template.ForType;

                SetDirtyState(false);
                RaiseUpdated(nameof(Presentations));
                Blocks.AddMany(template.Blocks.Select(ModuleBlockFactory.GetEditUI), true);
                MetadataList.AddMany(template.MetadataList, true);
            }
            catch (Exception ex)
            {
                this.ErrorNotification(SubSystemString.BadModule);

                Xaml.Get<ILogger>()
                    .Warn($"打开模组文件失败,文件名:{opendlg.FileName}，错误原因:{ex.Message}!");
            }
        }

        private async Task SaveTemplateImpl()
        {
            var target = Canvas;
            
            if (target is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Name))
            {
                await this.WarningNotification(SubSystemString.EmptyName);
                return;
            }
            
            
            if (string.IsNullOrEmpty(AuthorList))
            {
                await this.WarningNotification(SubSystemString.EmptyAuthor);
                return;
            }
            
            if (string.IsNullOrEmpty(For))
            {
                await this.WarningNotification(SubSystemString.EmptyFor);
                return;
            }

            // 保存模板的行为是:
            // 1) 判断当前的页面是否保存
            // 2) 选择文件
            // 3) 打开文件并赋值

            var savedlg = Studio.Save(SubSystemString.ModuleFilter, Studio.PngExt, PresentationName);

            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                var fileName = savedlg.FileName;
                var ms       = Xaml.CaptureToBuffer(target);
                var template = new ModuleTemplate
                {
                    Id            = _id,
                    Name          = _name,
                    Intro         = _intro,
                    AuthorList    = _authorList,
                    ContractList  = _contractList,
                    Organizations = _organizations,
                    Version       = ++TemplateVersion,
                    MetadataList  = MetadataList.ToList(),
                    Blocks = Blocks.Select(x => x.CreateInstance())
                                   .ToList(),
                    ForType = _forType,
                    For     = _for
                };
                var payload = JSON.Serialize(template);


                await PNG.Write(fileName, payload, ms);
                this.SuccessfulNotification(SubSystemString.OperationOfSaveIsSuccessful);
                SetDirtyState(false);
            }
            catch (Exception ex)
            {
                this.ErrorNotification(SubSystemString.BadModule);

                Xaml.Get<ILogger>()
                    .Warn($"保存模组文件失败,文件名:{savedlg.FileName}，错误原因:{ex.Message}!");
            }
        }

        #endregion

        #region Metadata

        private void DetectAll()
        {
            MetadataList.Clear();
            _metaHashSet.Clear();
            foreach (var block in Blocks)
            {
                DetectMetadata(block);
            }
        }

        private void DetectMetadata(IModuleBlockEditUI element)
        {
            if (element is GroupBlockEditUI gbe)
            {
                foreach (var subItem in gbe.Items)
                {
                    if (subItem is null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(subItem.Metadata))
                    {
                        continue;
                    }

                    AddOrUpdateMetadata(subItem.Name, subItem.Metadata);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(element.Metadata))
                {
                    return;
                }

                AddOrUpdateMetadata(element.Name, element.Metadata);
            }
        }

        private void AddOrUpdateMetadata(string name, string meta)
        {
            if (_metaHashSet.Add(meta))
            {
                MetadataList.Add(new MetadataCache
                {
                    Name         = name,
                    Id           = ID.Get(),
                    MetadataName = meta
                });
            }
        }

        #endregion

        #region Block

        private async Task NewBlockImpl()
        {
            var r = await NewBlockViewModel.New();

            if (!r.IsFinished)
            {
                return;
            }

            var element = r.Value;
            Blocks.Add(element);
            await EditBlockViewModel.Edit(element);
            RaiseUpdated(nameof(Presentations));
            DetectAll();
            SetDirtyState();
        }

        private async Task EditBlockImpl(ModuleBlockEditUI element)
        {
            var r = await EditBlockViewModel.Edit(element);
            RaiseUpdated(nameof(Presentations));

            if (!r.IsFinished)
            {
                return;
            }

            DetectAll();
            SetDirtyState();
        }

        private async Task RemoveBlockImpl(ModuleBlockEditUI element)
        {
            var r = await  this.Error(Language.RemoveAllItemText);

            if (!r)
            {
                return;
            }

            Blocks.Remove(element);
            RaiseUpdated(nameof(Presentations));
            DetectAll();
            SetDirtyState();
        }

        private void ShiftUpBlockImpl(ModuleBlockEditUI element)
        {
            Blocks.ShiftUp(element, () => RaiseUpdated(nameof(Presentations)));
        }

        private void ShiftDownBlockImpl(ModuleBlockEditUI element)
        {
            Blocks.ShiftDown(element, () => RaiseUpdated(nameof(Presentations)));
        }

        private async Task RemoveAllBlockImpl()
        {
            var r = await  this.Error(Language.RemoveAllItemText);

            if (!r)
            {
                return;
            }

            //
            // 清空
            Blocks.Clear();
            RaiseUpdated(nameof(Presentations));
            MetadataList.Clear();
            SetDirtyState();
        }

        #endregion

        private void RefreshMetadataListImpl()
        {
            DetectAll();
        }

        /// <summary>
        /// 获取或设置 <see cref="Id"/> 属性。
        /// </summary>
        public string Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }


        public FrameworkElement Canvas { get; set; }

        public string PresentationIntro => SubSystemString.GetIntro(_intro);
        public string PresentationContractList => SubSystemString.GetContractList(_contractList);
        public string PresentationAuthorList => SubSystemString.GetAuthor(_authorList);
        public string PresentationName => SubSystemString.GetName($"{_name}({_for})");

        /// <summary>
        /// 世界观
        /// </summary>
        public string For
        {
            get => _for;
            set
            {
                SetValue(ref _for, value);
                RaiseUpdated(nameof(PresentationName));
            }
        }

        /// <summary>
        /// 模组类型
        /// </summary>
        public DocumentType ForType
        {
            get => _forType;
            set => SetValue(ref _forType, value);
        }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro
        {
            get => _intro;
            set
            {
                SetValue(ref _intro, value);
                RaiseUpdated(nameof(PresentationIntro));
            }
        }

        /// <summary>
        /// 组织
        /// </summary>
        public string Organizations
        {
            get => _organizations;
            set => SetValue(ref _organizations, value);
        }

        /// <summary>
        /// 元数据列表
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<MetadataCache> MetadataList { get; init; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContractList
        {
            get => _contractList;
            set
            {
                SetValue(ref _contractList, value);
                RaiseUpdated(nameof(PresentationContractList));
            }
        }

        /// <summary>
        /// 作者列表
        /// </summary>
        public string AuthorList
        {
            get => _authorList;
            set
            {
                SetValue(ref _authorList, value);
                RaiseUpdated(nameof(PresentationAuthorList));
            }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                SetValue(ref _name, value);
                SetDirtyState();
                RaiseUpdated(nameof(PresentationName));
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="SelectedBlock"/> 属性。
        /// </summary>
        public ModuleBlockEditUI SelectedBlock
        {
            get => _selectedBlock;
            set
            {
                SetValue(ref _selectedBlock, value);
                EditBlockCommand.NotifyCanExecuteChanged();
                ShiftUpBlockCommand.NotifyCanExecuteChanged();
                ShiftDownBlockCommand.NotifyCanExecuteChanged();
                RemoveBlockCommand.NotifyCanExecuteChanged();
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="TemplateVersion"/> 属性。
        /// </summary>
        public int TemplateVersion
        {
            get => _templateVersion;
            set => SetValue(ref _templateVersion, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsPresentationPaneOpen"/> 属性。
        /// </summary>
        public bool IsPresentationPaneOpen
        {
            get => _isPresentationPaneOpen;
            set
            {
                SetValue(ref _isPresentationPaneOpen, value);
                if(value)RaiseUpdated(nameof(Presentations));
            }
        }

        /// <summary>
        /// 模组内容块集合。
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ModuleBlockEditUI> Blocks { get; }

        public IEnumerable<ModuleBlockDataUI> Presentations => Blocks.Select(x => ModuleBlockFactory.GetDataUI(x.CreateInstance()));

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand NewTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand OpenTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SaveTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModuleBlockEditUI> EditBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModuleBlockEditUI> RemoveBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleBlockEditUI> ShiftUpBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleBlockEditUI> ShiftDownBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand RemoveAllBlockCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand OpenPresentationPaneCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RefreshMetadataListCommand { get; }
    }
}