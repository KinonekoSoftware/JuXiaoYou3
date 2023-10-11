using System.Collections.ObjectModel;
using TimelineSetOC = System.Collections.ObjectModel.ObservableCollection<Acorisoft.Miga.Doc.Entities.Timelines.TimelineSet>;
using TimelineOC = System.Collections.ObjectModel.ReadOnlyObservableCollection<Acorisoft.Miga.Doc.Entities.Timelines.Timeline>;


namespace Acorisoft.Miga.Doc.Entities
{
    public class DetailPart : FixedDataPart
    {
        public DetailPart EnsureDataCorrect()
        {
            Appraise      ??= new ObservableCollection<Appraise>();
            Lines         ??= new ObservableCollection<string>();
            Relationships ??= new ObservableCollection<RelCopy>();
            SurveySets    ??= new ObservableCollection<SurveySet>();
            TimelineSets  ??= new TimelineSetOC();
            StickyNotes   ??= new ObservableCollection<StickyNote>();
            Keywords      ??= new ObservableCollection<string>();
            Albums        ??= new ObservableCollection<string>();
            Prototypes    ??= new ObservableCollection<Prototype>();
            return this;
        }
        public ObservableCollection<TimelineSet> TimelineSets { get; set; }
        public ObservableCollection<RelCopy> Relationships { get; set; }
        public ObservableCollection<string> Lines { get; set; }
        public ObservableCollection<SurveySet> SurveySets { get; set; }
        public ObservableCollection<Appraise> Appraise { get; set; }
        public ObservableCollection<StickyNote> StickyNotes { get; set; }
        public ObservableCollection<string> Albums { get; set; }
        public ObservableCollection<Prototype> Prototypes { get; set; }
        public ObservableCollection<string> Keywords { get; set; }
    }
}