using System.Diagnostics;

namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
{
    /// <summary>
    /// 元数据
    /// </summary>
    [DebuggerDisplay("{Name}-{Value}")]
    public class Metadata
    {
        /// <summary>
        /// 元数据的名字
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 元数据的值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 元数据的额外参数
        /// </summary>
        /// <remarks>
        /// 用于记录恢复元数据所需的额外内容（例如雷达图的图例。）
        /// </remarks>
        public string Parameters { get; set; }
        
        /// <summary>
        /// 元数据的类型。
        /// </summary>
        public MetadataKind Type { get; init; }
    }
}