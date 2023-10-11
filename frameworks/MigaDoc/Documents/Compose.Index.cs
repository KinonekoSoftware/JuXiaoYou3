using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Documents
{
    public class ComposeIndex : SnapshotObject
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDateTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifiedDateTime { get; set; }
        public ComposeType Type { get; init; }
        public string Summary { get; set; }
        public ObservableCollection<string> Keywords { get; set; }
    }
}