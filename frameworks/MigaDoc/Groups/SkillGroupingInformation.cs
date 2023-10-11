using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Groups
{
    public class SkillGroupingInformation : GroupingInformation
    {
        public ObservableCollection<DocumentIndexCopy> Members { get; init; }
    }
}