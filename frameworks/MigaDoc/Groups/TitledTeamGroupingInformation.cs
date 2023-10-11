using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Groups
{
    public class TitledMember
    {
        

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
        
        public ObservableCollection<DocumentIndexCopy> Members { get; init; }
    }
    
    public class TitledTeamGroupingInformation : GroupingInformation
    {
        public ObservableCollection<TitledMember> Members { get; init; }
    }
}