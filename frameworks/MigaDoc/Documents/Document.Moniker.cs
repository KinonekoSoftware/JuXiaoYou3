using System.Collections.ObjectModel;
using Acorisoft.Miga.Doc.Groups;

namespace Acorisoft.Miga.Doc.Documents
{
    public class Moniker
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public OldDocumentKind Type{ get; set; }
        
        public DocumentIndex Create()
        {
            return new DocumentIndex
            {
                Id               = Id,
                Name             = Name,
                DocumentType     = Type,
                CreatedDateTime  = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                Keywords         = new ObservableCollection<string>(),
                EntityType       = EntityType.Document,
                GroupType        = GroupType.None
            };
        }
    }
}