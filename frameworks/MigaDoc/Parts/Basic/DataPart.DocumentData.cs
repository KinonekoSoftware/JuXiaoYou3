using System.Collections.ObjectModel;


namespace Acorisoft.Miga.Doc.Parts
{
    public class DocumentData : FixedDataPart
    {
        public ObservableCollection<TimelineSet> TimelineSets { get; set; }
        public ObservableCollection<string> Album { get; set; }
        public ObservableCollection<Glimpse> Inspiration { get; set; }
        public ObservableCollection<SurveySet> SurveySets { get; set; }
        public ObservableCollection<object> StickyNote { get; set; }
        public ObservableCollection<object> Others { get; set; }
        public ObservableCollection<SimpleTextNode> SimpleTexts { get; set; }

    }

    public class SimpleTextNode
    {
        
        private string _content;

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
    }
}