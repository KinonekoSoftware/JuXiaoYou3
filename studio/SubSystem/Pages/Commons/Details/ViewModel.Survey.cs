using System.Linq;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class SurveyPartViewModelProxy : BindingProxy<SurveyPartViewModel>
    {
    }

    public class SurveyPartViewModel : IsolatedViewModel<DocumentEditorBase, PartOfSurvey>
    {
        private SurveySet _selectedSurveySet;

        public SurveyPartViewModel()
        {
            Sets          = new ObservableCollection<SurveySet>();
            ManageCommand = AsyncCommand(ManageImpl);
            ExportCommand = AsyncCommand(ExportImpl);
            ImportCommand = AsyncCommand(ImportImpl);
        }

        private async Task ManageImpl()
        {
            Op<List<SurveySet>> r;

            if (Sets.Count > 0)
            {
                r = await ManageSurveyViewModel.Edit(Sets);
            }
            else
            {
                r = await ManageSurveyViewModel.New();
            }

            if (!r.IsFinished)
            {
                return;
            }

            var c = r.Value;
            EnumerableExtensions.Diff(
                Sets, 
                c, 
                x => x.Id, 
                out var added, 
                out _, 
                out var removed);

            Detail.Items.AddMany(added);
            Detail.Items.RemoveMany(removed);
            Sets.AddMany(added);
            Sets.RemoveMany(removed);
            SelectedSurveySet = null;
            Save();
        }

        private async Task ExportImpl()
        {
            if (Sets.Count == 0)
            {
                await this.Warning(SubSystemString.NoDataToSave);
                return;
            }

            var savedlg = Studio.Save(SubSystemString.JsonFilter, "*.json");

            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            await JSON.ToFileAsync(Sets, savedlg.FileName);
        }

        private async Task ImportImpl()
        {
            var opendlg = Studio.Open(SubSystemString.JsonFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                var s = await JSON.FromFileAsync<ObservableCollection<SurveySet>>(opendlg.FileName);
                Sets.AddMany(s, true);
                Detail.Items.AddMany(s, true);
                Save();
            }
            catch
            {
                await this.WarningNotification(SubSystemString.BadFormat);
            }
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

            Sets.AddMany(Detail.Items, true);
            SelectedSurveySet = Sets.FirstOrDefault();
        }

        private void Save()
        {
            Owner.SetDirtyState();
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SurveySet> Sets { get; }

        public ObservableCollection<Survey> Surveys => SelectedSurveySet?.Items;

        /// <summary>
        /// 获取或设置 <see cref="SelectedSurveySet"/> 属性。
        /// </summary>
        public SurveySet SelectedSurveySet
        {
            get => _selectedSurveySet;
            set
            {
                SetValue(ref _selectedSurveySet, value);
                RaiseUpdated(nameof(Surveys));
            }
        }

        public AsyncRelayCommand ManageCommand { get; }
        public AsyncRelayCommand ExportCommand { get; }
        public AsyncRelayCommand ImportCommand { get; }
    }
}