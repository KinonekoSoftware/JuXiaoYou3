using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.Relatives;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class InTabViewModelProxy : BindingProxy<InTabViewModel>{}
    
    public abstract class InTabViewModel : TabStartupViewModel
    {
        public const string ToolsGroup = "global.tools";
        public const string FeatureGroup = "global.Feature";
        public const string PresetGroup  = "global.Preset";
        
        private readonly Subject<ViewModelBase> _onStart;
        private          MainFeature            _selectedFeature;
        private          ViewModelBase          _selectedViewModel;

        protected InTabViewModel()
        {
            _onStart = new Subject<ViewModelBase>();
            _onStart.ObserveOn(Scheduler)
                    .Subscribe(x => SelectedViewModel = x)
                    .DisposeWith(Collector);
            Features          = new ObservableCollection<MainFeature>();
            RunFeatureCommand = AsyncCommand<MainFeature>(RunFeature);
            Init();
        }

        private void Init()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            CreatePageFeature<CharacterRelationshipViewModel>(string.Empty, "__CharacterRelationship", null);
            CreateDialogFeature<DirectoryManagerViewModel>(string.Empty, "__DirectoryStatistic", null);
            CreateDialogFeature<RepairToolViewModel>(string.Empty, "global.repair", null);
            CreatePageFeature<KeywordViewModel>(string.Empty, "__Keywords", null);
            CreatePageFeature<BookmarkViewModel>(string.Empty, "__Bookmark", null);
        }

        protected void CreateDialogFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = true,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
        }


        protected void CreateDialogFeature<T>(string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = string.Empty,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = true,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
        }
        
        protected void CreateGalleryFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = true,
                Parameter = e
            };
            Features.Add(f);
        }
        
        protected void CreateGalleryFeature<T>(string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = string.Empty,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = true,
                Parameter = e
            };
            Features.Add(f);
        }
        
        protected void CreatePageFeature<T>(string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = string.Empty,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
        }

        protected void CreatePageFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
        }

        private async Task RunFeature(MainFeature feature)
        {
            await MainFeature.Run(feature, DialogService, Controller, _onStart);
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<MainFeature> RunFeatureCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<MainFeature> Features { get; }

        /// <summary>
        /// 获取或设置 <see cref="SelectedViewModel"/> 属性。
        /// </summary>
        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel?.Suspend();
                SetValue(ref _selectedViewModel, value);
                _selectedViewModel.Resume();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedFeature"/> 属性。
        /// </summary>
        public MainFeature SelectedFeature
        {
            get => _selectedFeature;
            set
            {
                SetValue(ref _selectedFeature, value);
                RunFeature(value)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}