using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Entities.Organizations
{
    public class Organization
    {
        
        private string _summary;
        private string _avatar;

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar{ get; set; }
        
        /// <summary>
        /// 获取或设置唯一标识符
        /// </summary>
        public string Id { get; init; }
        
        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
        
        public ObservableCollection<string> Backgrounds { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<StickyNote> Story { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OrganizationMember> Members { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OrganizationBranch> Branches { get; init; }
    }
}