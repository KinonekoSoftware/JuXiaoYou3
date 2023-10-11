using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class SentencePartViewModel : DetailViewModel<PartOfSentence, Sentence>
    {
        public SentencePartViewModel()
        {
            ProjectEngine = Studio.Engine<ProjectEngine>();
            EditCommand   = AsyncCommand<Sentence>(EditItem);
        }

        protected override async Task AddItem()
        {
            var r1 = await SingleLineViewModel.String(SubSystemString.EditValueTitle);

            if (!r1.IsFinished)
            {
                return;
            }
            
            var appraise = new Sentence
            {
                Id      = ID.Get(),
                Source  = Owner.Cache,
                Content = r1.Value 
            };
            
            ProjectEngine.AddSentence(appraise);
            Collection.Add(appraise);
        }
        
        protected async Task EditItem(Sentence item)
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
            ProjectEngine.AddSentence(item);
            SaveOperation();
        }
        

        protected override async Task RemoveItem(Sentence item)
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
            ProjectEngine.RemoveSentence(item);
            SaveOperation();
        }

        protected override void ShiftDownItem(Sentence item)
        {
            Collection.ShiftDown(item);
            SaveOperation();
        }

        protected override void ShiftUpItem(Sentence item)
        {
            Collection.ShiftUp(item);
            SaveOperation();
        }
        
        protected override void OnInitialize(ICollection<Sentence> collection)
        {
            var i = ProjectEngine.GetSentences(Owner.Cache);
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
        public AsyncRelayCommand<Sentence> EditCommand { get; }
        public ProjectEngine ProjectEngine { get; }
    }
}