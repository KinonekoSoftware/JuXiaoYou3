using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Entities.Organizations
{
    public class OrganizationBranch
    {
        public string Id { get; init; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public ObservableCollection<OrganizationMember> Members { get; init; }
    }
}