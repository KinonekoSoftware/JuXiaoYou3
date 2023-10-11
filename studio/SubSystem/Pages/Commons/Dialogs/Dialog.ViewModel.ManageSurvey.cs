using System.Linq;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ManageSurveyViewModel : ImplicitDialogVM
    {
        private SurveySet _selectedSurveySet;
        private Survey _selectedSurvey;

        public ManageSurveyViewModel()
        {
            SurveySets = new ObservableCollection<SurveySet>();
            AddSurveySetCommand = AsyncCommand(AddSurveySetImpl);
            AddSurveyCommand = AsyncCommand<SurveySet>(AddSurveyImpl);
            ShiftUpSurveySetCommand = Command<SurveySet>(ShiftUpSurveySetImpl, HasItem);
            ShiftUpSurveyCommand = Command<Survey>(ShiftUpSurveyImpl, HasItem);
            ShiftDownSurveySetCommand = Command<SurveySet>(ShiftDownSurveySetImpl, HasItem);
            ShiftDownSurveyCommand = Command<Survey>(ShiftDownSurveyImpl, HasItem);
            EditSurveySetCommand = AsyncCommand<SurveySet>(EditSurveySetImpl, HasItem);
            EditSurveyCommand = AsyncCommand<Survey>(EditSurveyImpl, HasItem);
            RemoveSurveySetCommand = AsyncCommand<SurveySet>(RemoveSurveySetImpl, HasItem);
            RemoveSurveyCommand = AsyncCommand<Survey>(RemoveSurveyImpl, HasItem);
        }

        public static Task<Op<List<SurveySet>>> New()
        {
            return DialogService().Dialog<List<SurveySet>, ManageSurveyViewModel>();
        }

        public static Task<Op<List<SurveySet>>> Edit(ObservableCollection<SurveySet> source)
        {
            if (source is not null)
            {
                return DialogService().Dialog<List<SurveySet>, ManageSurveyViewModel>(new Parameter
                {
                    Args = new object[]
                    {
                        source
                    }
                });
            }

            return New();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            if (a[0] is IEnumerable<SurveySet> s)
            {
                SurveySets.AddMany(s);
                SelectedSurveySet = SurveySets.FirstOrDefault();
            }
            base.OnStart(parameter);
        }


        private async Task EditSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            await NewSurveyViewModel.Edit(item);
        }

        private async Task EditSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            await NewSurveyViewModel.Edit(item);
        }

        private async Task RemoveSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            SelectedSurveySet.Items.Remove(item);
            Surveys.Remove(item);
        }

        private async Task RemoveSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            SurveySets.Remove(item);
        }

        private void ShiftDownSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            Surveys.ShiftDown(item);
            SelectedSurveySet.Items.ShiftDown(item);

        }

        private void ShiftDownSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            SurveySets.ShiftDown(item);
        }

        private void ShiftUpSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            Surveys.ShiftUp(item);
            SelectedSurveySet.Items.ShiftUp(item);

        }

        private void ShiftUpSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            SurveySets.ShiftUp(item);
        }

        private async Task AddSurveyImpl(SurveySet item)
        {
            var r = await NewSurveyViewModel.New();

            if (!r.IsFinished)
            {
                return;
            }

            item.Items
                .Add(r.Value);
        }

        private async Task AddSurveySetImpl()
        {
            var r = await NewSurveyViewModel.NewSet();

            if (!r.IsFinished)
            {
                return;
            }

            SurveySets.Add(r.Value);
        }

        protected override bool IsCompleted() => true;

        protected override void Finish()
        {
            Result = new List<SurveySet>(SurveySets.ToArray());
        }

        public override void Start()
        {
            InternalCommands.ForEach(x => x.NotifyCanExecuteChanged());
        }

        protected override string Failed() => SubSystemString.Unknown;

        /// <summary>
        /// 获取或设置 <see cref="SelectedSurvey"/> 属性。
        /// </summary>
        public Survey SelectedSurvey
        {
            get => _selectedSurvey;
            set
            {
                SetValue(ref _selectedSurvey, value);
                InternalCommands.ForEach(x => x.NotifyCanExecuteChanged());
            }
        }

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
                InternalCommands.ForEach(x => x.NotifyCanExecuteChanged());
            } 
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<SurveySet> SurveySets { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<Survey> Surveys => SelectedSurveySet?.Items;

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddSurveySetCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SurveySet> AddSurveyCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<SurveySet> ShiftUpSurveySetCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Survey> ShiftUpSurveyCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<SurveySet> ShiftDownSurveySetCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Survey> ShiftDownSurveyCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SurveySet> RemoveSurveySetCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Survey> RemoveSurveyCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SurveySet> EditSurveySetCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Survey> EditSurveyCommand { get; }
    }
}