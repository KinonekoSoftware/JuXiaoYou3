// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Documents
{
    /// <summary>
    /// <see cref="Document"/> 类型表示一个文档。
    /// </summary>
    public class Document : StorageObject
    {

        /// <summary>
        /// 获取或设置当前的文档类型
        /// </summary>
        public OldDocumentKind Type { get; init; }

        /// <summary>
        /// 获取或设置当前的部件
        /// </summary>
        public DataPartCollection Parts { get; set; }

        /// <summary>
        /// 获取或设置当前的元数据
        /// </summary>
        public MetadataCollection Metas { get; set; }
    }
}