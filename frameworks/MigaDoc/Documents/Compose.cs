using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Documents
{
    public class Compose 
    {
        public string Id { get; init; }
        public ObservableCollection<ComposeVersion> Drafts { get; init; }
        public ObservableCollection<ComposeVersion> RecycleBin { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Current"/> 属性。
        /// </summary>
        public ComposeVersion Current{ get; set; }
    }

    public class ComposeVersion 
    {
        private string   _name;
        private DateTime _modifiedDateTime;
        private string   _hash;

        /// <summary>
        /// 获取或设置 <see cref="Hash"/> 属性。
        /// </summary>
        public string Hash{ get; set; }
        
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="ModifiedDateTime"/> 属性。
        /// </summary>
        public DateTime ModifiedDateTime{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
        
        public DateTime CreatedDateTime { get; init; }
        public string Content { get; set; }
    }
}