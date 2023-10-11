using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class StickyNotePartViewModelProxy : BindingProxy<StickyNotePartViewModel>
    {
    }

    public class StickyNotePartViewModel : IsolatedViewModel<DocumentEditorBase,PartOfStickyNote>
    {
        private StickyNote _selected;
        
        public StickyNotePartViewModel()
        {
            Collection       = new ObservableCollection<StickyNote>();
            AddCommand       = AsyncCommand(AddImpl);
            RemoveCommand    = AsyncCommand<StickyNote>(RemoveImpl);
            ShiftUpCommand   = Command<StickyNote>(ShiftUpImpl);
            ShiftDownCommand = Command<StickyNote>(ShiftDownImpl);
        }

        public override void Start()
        {
            base.Start();

            if (Detail.Items is null)
            {
                Xaml.Get<ILogger>()
                    .Warn("PartOf为空");
                return;
            }

            Collection.AddMany(Detail.Items);
            RaiseUpdated(nameof(NotEmpty));
            RaiseUpdated(nameof(HasSelected));
        }

        private async Task AddImpl()
        {
            var title = await SingleLineViewModel.String(SubSystemString.EditNameTitle);

            if (!title.IsFinished)
            {
                return;
            }

            var now = DateTime.Now;
            var item = new StickyNote
            {
                Id             = ID.Get(),
                Title          = title.Value,
                TimeOfCreated  = now,
                TimeOfModified = now
            };
            Collection.Add(item);
            Detail.Items.Add(item);
            Selected = item;
            RaiseUpdated(nameof(NotEmpty));
            Save();
        }

        private async Task RemoveImpl(StickyNote stickyNote)
        {
            if (stickyNote is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (ReferenceEquals(Selected, stickyNote))
            {
                Selected = null;
            }

            Collection.Remove(stickyNote);
            Detail.Items.Remove(stickyNote);
            RaiseUpdated(nameof(NotEmpty));
            Save();
        }

        private void ShiftDownImpl(StickyNote stickyNote)
        {
            if (stickyNote is null)
            {
                return;
            }

            Collection.ShiftDown(stickyNote);
            Detail.Items.ShiftDown(stickyNote);
            Save();
        }

        private void ShiftUpImpl(StickyNote stickyNote)
        {
            if (stickyNote is null)
            {
                return;
            }

            Collection.ShiftUp(stickyNote);
            Detail.Items.ShiftUp(stickyNote);
            Save();
        }

        private void Save()
        {
            Owner.SetDirtyState();
        }

        /// <summary>
        /// 获取或设置 <see cref="NotEmpty"/> 属性。
        /// </summary>
        public bool NotEmpty => Collection.Count > 0;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<StickyNote> Collection { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public StickyNote Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);
                RaiseUpdated(nameof(HasSelected));
            }
        }

        public bool HasSelected => Selected is not null;

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<StickyNote> ShiftUpCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<StickyNote> ShiftDownCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<StickyNote> RemoveCommand { get; }
    }
}