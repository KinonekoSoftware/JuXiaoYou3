using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class AppraisePartViewModel : DetailViewModel<PartOfAppraise, Appraise>
    {
        public AppraisePartViewModel()
        {
            ProjectEngine = Studio.Engine<ProjectEngine>();
            EditCommand   = AsyncCommand<Appraise>(EditItem);
        }

        protected override async Task AddItem()
        {
            var r = await SubSystem.SelectExclude(DocumentType.Character, new HashSet<string>
            {
                Owner.Cache
                     .Id
            });

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await MultiLineViewModel.String(SubSystemString.EditValueTitle);

            if (!r1.IsFinished)
            {
                return;
            }
            
            var appraise = new Appraise
            {
                Id      = ID.Get(),
                Target  = r.Value,
                Source  = Owner.Cache,
                Content = r1.Value 
            };
            
            ProjectEngine.AddAppraise(appraise);
            Collection.Add(appraise);
        }
        
        protected async Task EditItem(Appraise item)
        {
            if (item is null)
            {
                return;
            }

            var r1 = await MultiLineViewModel.String(SubSystemString.EditValueTitle, item.Content);

            if (!r1.IsFinished)
            {
                return;
            }
            
            item.Content = r1.Value;
            ProjectEngine.AddAppraise(item);
            SaveOperation();
        }
        

        protected override async Task RemoveItem(Appraise item)
        {
            if (item is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Collection.Remove(item);
            ProjectEngine.RemoveAppraise(item);
            SaveOperation();
        }

        protected override void ShiftDownItem(Appraise item)
        {
            Collection.ShiftDown(item);
            SaveOperation();
        }

        protected override void ShiftUpItem(Appraise item)
        {
            Collection.ShiftUp(item);
            SaveOperation();
        }
        
        protected override void OnInitialize(ICollection<Appraise> collection)
        {
            var i = ProjectEngine.GetAppraises(Owner.Cache);
            collection.AddMany(i, true);
            
        }

        protected override void UpdateCommandState()
        {
            EditCommand.NotifyCanExecuteChanged();
        }

        protected new void SaveOperation()
        {
            UpdateCollectionState();
            UpdateCommandState();
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Appraise> EditCommand { get; }
        public ProjectEngine ProjectEngine { get; }
    }
}