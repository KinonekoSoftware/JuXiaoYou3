namespace Acorisoft.Miga.Doc.Groups
{
    using System.Collections.ObjectModel;

    public class TeamGroupingInformation : GroupingInformation
    {
        public ObservableCollection<DocumentIndexCopy> Members { get; init; }
    }
}