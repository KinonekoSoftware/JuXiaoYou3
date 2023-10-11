using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public abstract class TargetBlockDataUI : ModuleBlockDataUI, ITargetBlockDataUI
    {
        private string _targetName;
        private string _targetSource;
        private string _targetThumbnail;

        protected TargetBlockDataUI(TargetBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            TargetBlock     = block;
            TargetName      = block.TargetName;
            TargetSource    = block.TargetSource;
            TargetThumbnail = block.TargetThumbnail;
            OpenCommand     = new RelayCommand(OpenImpl);
        }

        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        private void OpenImpl()
        {
            if (string.IsNullOrEmpty(TargetSource))
            {
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName        = "explorer.exe",
                Arguments       = TargetSource
            });
        }

        /// <summary>
        /// 目标内容块
        /// </summary>
        protected TargetBlock TargetBlock { get; }

        /// <summary>
        /// 获取或设置 <see cref="TargetThumbnail"/> 属性。
        /// </summary>
        public string TargetThumbnail
        {
            get => _targetThumbnail;
            set
            {
                SetValue(ref _targetThumbnail, value);
                TargetBlock.TargetThumbnail = value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetSource"/> 属性。
        /// </summary>
        public string TargetSource
        {
            get => _targetSource;
            set
            {
                TargetBlock.TargetSource = value;
                SetValue(ref _targetSource, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetName"/> 属性。
        /// </summary>
        public string TargetName
        {
            get => _targetName;
            set
            {
                TargetBlock.TargetName = value;
                SetValue(ref _targetName, value);
            }
        }

        public RelayCommand OpenCommand { get; }
    }

    public class AudioBlockDataUI : TargetBlockDataUI
    {
        public AudioBlockDataUI(AudioBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        public AudioBlockDataUI(AudioBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        private void SelectImpl()
        {
            var opendlg = Studio.Open(SubSystemString.AudioFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            TargetName   = Path.GetFileNameWithoutExtension(opendlg.FileName);
            TargetSource = opendlg.FileName;
        }

        public RelayCommand SelectCommand { get; }
    }

    public class VideoBlockDataUI : TargetBlockDataUI
    {
        public VideoBlockDataUI(VideoBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        public VideoBlockDataUI(VideoBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        private void SelectImpl()
        {
            var opendlg = Studio.Open(SubSystemString.VideoFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            TargetName   = Path.GetFileNameWithoutExtension(opendlg.FileName);
            TargetSource = opendlg.FileName;
        }

        public RelayCommand SelectCommand { get; }
    }

    public class FileBlockDataUI : TargetBlockDataUI
    {
        public FileBlockDataUI(FileBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        public FileBlockDataUI(FileBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        private void SelectImpl()
        {
            var opendlg = Studio.Open(SubSystemString.AllFileFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            TargetName   = Path.GetFileNameWithoutExtension(opendlg.FileName);
            TargetSource = opendlg.FileName;
        }

        public RelayCommand SelectCommand { get; }
    }

    public class MusicBlockDataUI : TargetBlockDataUI
    {
        public MusicBlockDataUI(MusicBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            SelectCommand = new AsyncRelayCommand(SelectImpl);
        }

        public MusicBlockDataUI(MusicBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
            SelectCommand = new AsyncRelayCommand(SelectImpl);
        }

        private async Task SelectImpl()
        {
            var opendlg = Studio.Open(SubSystemString.MusicFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            await MusicUtilities.AddMusic(opendlg.FileName, Studio.Engine<MusicEngine>()
                , x =>
                {
                    Dispatcher.CurrentDispatcher
                              .Invoke(() =>
                              {
                                  TargetThumbnail = x.Cover;
                                  TargetName      = x.Name;
                                  TargetSource    = x.Path;
                              });
                });
        }

        public AsyncRelayCommand SelectCommand { get; }
    }

    public class ImageBlockDataUI : TargetBlockDataUI
    {
        public ImageBlockDataUI(ImageBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        public ImageBlockDataUI(ImageBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
            SelectCommand = new RelayCommand(SelectImpl);
        }

        private void SelectImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            TargetName   = Path.GetFileNameWithoutExtension(opendlg.FileName);
            TargetSource = opendlg.FileName;
        }

        public RelayCommand SelectCommand { get; }
    }

    public class ReferenceBlockDataUI : ModuleBlockDataUI, ITargetBlockDataUI, IReferenceBlock
    {
        private string _targetName;
        private string _targetSource;
        private string _targetThumbnail;

        public ReferenceBlockDataUI(ReferenceBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            TargetBlock     = block;
            TargetName      = block.TargetName;
            TargetSource    = block.TargetSource;
            TargetThumbnail = block.TargetThumbnail;
            OpenCommand     = new RelayCommand(OpenImpl);
            SelectCommand   = new AsyncRelayCommand(SelectImpl);
        }

        public ReferenceBlockDataUI(ReferenceBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
            OpenCommand   = new RelayCommand(OpenImpl);
            SelectCommand = new AsyncRelayCommand(SelectImpl);
        }

        private void OpenImpl()
        {
            if (string.IsNullOrEmpty(TargetSource))
            {
                return;
            }

            DocumentUtilities.OpenDocument(TargetSource);
        }

        private async Task SelectImpl()
        {
            var r = await SubSystem.Select(Studio.Engine<DocumentEngine>()
                                                               .GetCaches());

            if (!r.IsFinished)
            {
                return;
            }

            var v = r.Value;
            TargetThumbnail = v.Avatar;
            TargetName      = v.Name;
            TargetSource    = v.Id;
        }

        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        /// <summary>
        /// 目标内容块
        /// </summary>
        protected ReferenceBlock TargetBlock { get; }

        /// <summary>
        /// 获取或设置 <see cref="TargetThumbnail"/> 属性。
        /// </summary>
        public string TargetThumbnail
        {
            get => _targetThumbnail;
            set
            {
                SetValue(ref _targetThumbnail, value);
                TargetBlock.TargetThumbnail = value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetSource"/> 属性。
        /// </summary>
        public string TargetSource
        {
            get => _targetSource;
            set
            {
                TargetBlock.TargetSource = value;
                SetValue(ref _targetSource, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetName"/> 属性。
        /// </summary>
        public string TargetName
        {
            get => _targetName;
            set
            {
                TargetBlock.TargetName = value;
                SetValue(ref _targetName, value);
            }
        }

        public RelayCommand OpenCommand { get; }
        public AsyncRelayCommand SelectCommand { get; }
    }

    public class AudioBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public AudioBlockEditUI(AudioBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new AudioBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class VideoBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public VideoBlockEditUI(VideoBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new VideoBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class FileBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public FileBlockEditUI(FileBlock block) : base(block)
        {
        }

        public override FileBlock CreateInstance()
        {
            return new FileBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class MusicBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public MusicBlockEditUI(MusicBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new MusicBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class ImageBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public ImageBlockEditUI(ImageBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new ImageBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class ReferenceBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public ReferenceBlockEditUI(IReferenceBlock block) : base(block)
        {
            TargetName      = block.TargetName;
            TargetSource    = block.TargetSource;
            TargetThumbnail = block.TargetThumbnail;
        }

        private ReferenceSource _dataSource;
        private string          _targetName;
        private string          _targetSource;
        private string          _targetThumbnail;

        /// <summary>
        /// 获取或设置 <see cref="TargetThumbnail"/> 属性。
        /// </summary>
        public string TargetThumbnail
        {
            get => _targetThumbnail;
            set => SetValue(ref _targetThumbnail, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetSource"/> 属性。
        /// </summary>
        public string TargetSource
        {
            get => _targetSource;
            set => SetValue(ref _targetSource, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetName"/> 属性。
        /// </summary>
        public string TargetName
        {
            get => _targetName;
            set => SetValue(ref _targetName, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="DataSource"/> 属性。
        /// </summary>
        public ReferenceSource DataSource
        {
            get => _dataSource;
            set => SetValue(ref _dataSource, value);
        }

        public override ModuleBlock CreateInstance()
        {
            return new ReferenceBlock()
            {
                Id         = Id,
                Name       = Name,
                Metadata   = Metadata,
                ToolTips   = ToolTips,
                DataSource = DataSource,
            };
        }
    }
}