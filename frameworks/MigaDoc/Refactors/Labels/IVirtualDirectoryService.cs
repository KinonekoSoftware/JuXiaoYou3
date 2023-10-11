namespace Acorisoft.Miga.Doc.Labels
{
    public interface IVirtualDirectoryService<TLabel, TMapping, TDirectory>
        where TLabel : class, IObjectLabel
        where TMapping : class, IObjectMapping
        where TDirectory : class, IVirtualDirectory
    {
        /// <summary>
        /// 指定一个标签ID，获取该标签ID下的所有对象映射。
        /// </summary>
        /// <param name="id">指定标签的唯一标识符。</param>
        /// <returns>返回一个对象映射枚举。</returns>
        IEnumerable<IObjectMapping> GetFiles(string id);

        /// <summary>
        /// 指定一个标签ID，获取该标签ID下的所有对象映射。
        /// </summary>
        /// <param name="label">指定的标签。</param>
        /// <returns>返回一个对象映射枚举。</returns>
        IEnumerable<IObjectMapping> GetFiles(IObjectLabel label);

        /// <summary>
        /// 用于存放映射的集合
        /// </summary>
        IObjectMappingCollection<TMapping> Mappings { get; }

        /// <summary>
        /// 用于存放虚拟目录的集合
        /// </summary>
        IVirtualDirectoryCollection<TDirectory> Directories { get; }

        /// <summary>
        /// 用于存放当前标签的集合
        /// </summary>
        IObjectLabelCollection<TLabel> Labels { get; }
    }
}