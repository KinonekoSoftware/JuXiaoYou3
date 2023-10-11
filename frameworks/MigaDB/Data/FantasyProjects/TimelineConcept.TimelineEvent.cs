using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public class TimelineEvent : TimelineConcept
    {
        private DocumentCache _geography;
        private string        _backgroundInformation;
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<DocumentCache> Characters { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="BackgroundInformation"/> 属性。
        /// </summary>
        public string BackgroundInformation
        {
            get => _backgroundInformation;
            set => SetValue(ref _backgroundInformation, value);
        }
        /// <summary>
        /// 故事发生地点
        /// </summary>
        public DocumentCache Geography
        {
            get => _geography;
            set => SetValue(ref _geography, value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<EventElement> Stories { get; init; }
    }
}