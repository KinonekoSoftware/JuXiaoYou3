using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe.Dialogs;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class FantasyProjectTimelineViewModel : TabViewModel
    {
        public FantasyProjectTimelineViewModel()
        {
            Timelines       = new ObservableCollection<TimelineConcept>();
            ProjectEngine   = Studio.Engine<ProjectEngine>();
            
            AddTimelineAgeCommand         = AsyncCommand(AddTimelineAgeImpl);
            AddTimelineAgeBeforeCommand   = AsyncCommand<TimelineConcept>(AddTimelineAgeBeforeImpl);
            AddTimelineAgeAfterCommand    = AsyncCommand<TimelineConcept>(AddTimelineAgeAfterImpl);
            AddTimelineAgeCommand         = AsyncCommand(AddTimelineAgeImpl);
            AddTimelineEventBeforeCommand = AsyncCommand<TimelineConcept>(AddTimelineEventBeforeImpl);
            AddTimelineEventAfterCommand  = AsyncCommand<TimelineConcept>(AddTimelineEventAfterImpl);
            AddTimelineEventCommand       = AsyncCommand(AddTimelineEventImpl);
            RemoveTimelineCommand         = AsyncCommand<TimelineConcept>(RemoveTimelineImpl);
            EditTimelineCommand           = AsyncCommand<TimelineConcept>(EditTimelineImpl);
            ShiftUpTimelineCommand        = Command<TimelineConcept>(ShiftUpTimelineImpl);
            ShiftDownTimelineCommand      = Command<TimelineConcept>(ShiftDownTimelineImpl);
        }
        
        
        protected override void OnStart()
        {
            InitializeTimelines();
        }
        
        #region V1

        private void InitializeTimelines()
        {
            var inside     = ProjectEngine.GetTimelines();
            var dictionary = new Dictionary<string, TimelineConcept>();
            
            foreach (var t in inside)
            {
                dictionary.TryAdd(t.Id, t);
            }
            
            var first  = dictionary.FirstOrDefault(x => string.IsNullOrEmpty(x.Value
                                                                              .LastItem))
                                   .Value;
            var breakdownCounter = 0;

            if(first is null)
            {
                return;
            }
            
            Timelines.Add(first);

            while (!string.IsNullOrEmpty(first.NextItem))
            {
                breakdownCounter++;
                
                if (dictionary.TryGetValue(first.NextItem, out var c))
                {
                    Timelines.Add(c);
                    dictionary.Remove(first.NextItem);
                    first = c;
                }

                if (breakdownCounter > dictionary.Count ||
                    breakdownCounter > 100000)
                {
                    break;
                }
            }
            
        }
        
        private async Task AddTimelineConceptImpl(bool isAge, TimelineConcept target, bool after)
        {
            TimelineConcept concept;

            if (isAge)
            {
                var r = await NewTimelineViewModel.NewAge();

                if (!r.IsFinished)
                {
                    return;
                }

                concept = r.Value;
            }
            else
            {
                var r = await NewTimelineViewModel.NewEvent();

                if (!r.IsFinished)
                {
                    return;
                }

                concept = r.Value;
            }

            if (Timelines.Count == 0)
            {
                //
                // add first node
                ProjectEngine.AddTimeline(concept);
                Timelines.Add(concept);
                return;
            }

            if (target is null)
            {
                var last = Timelines.Last();
                last.NextItem    = concept.Id;
                concept.LastItem = last.Id;
                ProjectEngine.AddTimeline(last);
                ProjectEngine.AddTimeline(concept);
                Timelines.Add(concept);
                return;
            }
            
            var index = Timelines.IndexOf(target);

            if (index == -1)
            {
                var last = Timelines.Last();
                last.NextItem = concept.Id;
                ProjectEngine.AddTimeline(last);
                ProjectEngine.AddTimeline(concept);
                Timelines.Add(concept);
                return;
            }

            if (after)
            {
                if (index == Timelines.Count - 1)
                {
                    /*
                     *  A
                     *  |
                     *  target
                     *  | < ----- concept
                     *  |
                     *  null
                     */

                    concept.LastItem = target.Id;
                    target.NextItem  = concept.Id;
                    ProjectEngine.AddTimeline(target);
                    ProjectEngine.AddTimeline(concept);
                    Timelines.Insert(index + 1, concept);
                }
                else
                {
                    /*
                     *  A
                     *  |
                     *  target
                     *  | < ----- concept
                     *  |
                     *  B
                     */
                    var b = Timelines[index + 1];
                    concept.NextItem = b.LastItem;
                    concept.LastItem = target.Id;
                    target.NextItem  = concept.Id;
                    b.LastItem       = concept.Id;
                    ProjectEngine.AddTimeline(target);
                    ProjectEngine.AddTimeline(concept);
                    ProjectEngine.AddTimeline(b);
                    Timelines.Insert(index + 1, concept);
                }
                return;
            }
            
            if (index == 0)
            {
                /*
                 *  null
                 *  |
                 *  | < ----- concept
                 *  target
                 *  |
                 *  b
                 */

                concept.NextItem = target.Id;
                target.LastItem  = concept.Id;
                ProjectEngine.AddTimeline(target);
                ProjectEngine.AddTimeline(concept);
                Timelines.Insert(index, concept);
            }
            else
            {
                /*
                 *  a
                 *  |
                 *  | < ----- concept
                 *  target
                 *  |
                 *  b
                 */
                var a = Timelines[index - 1];

                concept.NextItem = target.Id;
                concept.LastItem = a.Id;
                a.NextItem       = concept.Id;
                target.LastItem  = concept.Id;
                ProjectEngine.AddTimeline(target);
                ProjectEngine.AddTimeline(a);
                ProjectEngine.AddTimeline(concept);
                Timelines.Insert(index, concept);
            }
        }

        private Task AddTimelineAgeImpl()
        {
            return AddTimelineConceptImpl(true, null, true);
        }
        
        private Task AddTimelineAgeBeforeImpl(TimelineConcept target)
        {
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            return AddTimelineConceptImpl(true, target, false);
        }
        
        private Task AddTimelineAgeAfterImpl(TimelineConcept target)
        {
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            return AddTimelineConceptImpl(true, target, true);
        }
        
        private Task AddTimelineEventBeforeImpl(TimelineConcept target)
        { 
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            return AddTimelineConceptImpl(false, target, false);
        }
        
        private Task AddTimelineEventAfterImpl(TimelineConcept target)
        {
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            
            return AddTimelineConceptImpl(false, target, true);
        }
        
        private Task AddTimelineEventImpl()
        {
            return AddTimelineConceptImpl(false, null, true);
        }
        
        private void ShiftDownTimelineImpl(TimelineConcept c)
        {
            if (c is null)
            {
                return;
            }

            var index = Timelines.IndexOf(c);

            if (index == -1)
            {
                return;
            }
            var b = index == 0 ? null : Timelines[index - 1];
            var a = index == Timelines.Count - 1 ? null : Timelines[index + 1];

            if (b is not null)
            {
                b.NextItem = a?.Id;
            }

            c.LastItem = a?.Id;
            c.NextItem = a?.NextItem;

            if (a is not null)
            {
                a.LastItem = b?.Id;
                a.NextItem = c.Id;
            }


            Timelines.ShiftDown(c);
            ProjectEngine.AddTimeline(b);
            ProjectEngine.AddTimeline(a);
            ProjectEngine.AddTimeline(c);
        }
        
        private void ShiftUpTimelineImpl(TimelineConcept c)
        {
            if (c is null)
            {
                return;
            }

            var index = Timelines.IndexOf(c);

            if (index == -1)
            {
                return;
            }

            var b = index == 0 ? null : Timelines[index - 1];
            var a = index == Timelines.Count - 1 ? null : Timelines[index + 1];

            c.LastItem = b?.LastItem;
            c.NextItem = b?.Id;

            if (b is not null)
            {
                b.LastItem = c.Id;
                b.NextItem = a?.Id;
            }

            if (a is not null)
            {
                a.LastItem = b?.Id;
            }
            Timelines.ShiftUp(c);
            ProjectEngine.AddTimeline(c);
            ProjectEngine.AddTimeline(b);
            ProjectEngine.AddTimeline(a);
        }

        private async Task RemoveTimelineImpl(TimelineConcept concept)
        {
            if (concept is null)
            {
                return;
            }
            
            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }


            var index = Timelines.IndexOf(concept);

            if (index == -1)
            {
                return;
            }

            if (Timelines.Count == 1)
            {
                // only-one
                ProjectEngine.RemoveTimeline(concept);
                Timelines.Remove(concept);
                return;
            }

            TimelineConcept last;
            TimelineConcept next;
            
            if (index == 0)
            {
                // first
                next = Timelines[1];
                next.LastItem = null;
                ProjectEngine.AddTimeline(next);
                ProjectEngine.RemoveTimeline(concept);
                Timelines.Remove(concept);
                return;
            }

            if (index == Timelines.Count - 1)
            {
                // last
                last          = Timelines[index - 1];
                last.NextItem = null;

                ProjectEngine.AddTimeline(last);
                ProjectEngine.RemoveTimeline(concept);
                Timelines.Remove(concept);
                return;
            }

            last = Timelines[index - 1];
            next = Timelines[index + 1];

            last.NextItem = next.Id;
            next.LastItem = last.Id;
                
                
            ProjectEngine.AddTimeline(last);
            ProjectEngine.AddTimeline(next);
            ProjectEngine.RemoveTimeline(concept);
            Timelines.Remove(concept);
            
        }
        
        private async Task EditTimelineImpl(TimelineConcept concept)
        {
            if (concept is null)
            {
                return;
            }

            var r = await NewTimelineViewModel.Edit(concept);

            if (!r.IsFinished)
            {
                return;
            }
            
            ProjectEngine.AddTimeline(r.Value);
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddTimelineAgeCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineAgeAfterCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineAgeBeforeCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddTimelineEventCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineEventAfterCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineEventBeforeCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<TimelineConcept> ShiftUpTimelineCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<TimelineConcept> ShiftDownTimelineCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> EditTimelineCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> RemoveTimelineCommand { get; }
        
        public ObservableCollection<TimelineConcept> Timelines { get; }

        #endregion
        
        public ProjectEngine ProjectEngine { get; }
    }
}