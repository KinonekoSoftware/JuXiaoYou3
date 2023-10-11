
namespace Acorisoft.Miga.Doc.Metadatas
{
    /// <summary>
    /// <see cref="Metadata"/> 类型表示一个元数据。
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// 当前对象名。
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 获取或设置当前元数据的值。
        /// </summary>
        public string Value { get; set; }
    }
}