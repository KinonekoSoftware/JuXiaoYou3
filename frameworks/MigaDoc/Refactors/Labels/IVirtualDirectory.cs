namespace Acorisoft.Miga.Doc.Labels
{
    /// <summary>
    /// <see cref="IVirtualDirectory"/> 接口用于实现抽象的目录。
    /// </summary>
    public interface IVirtualDirectory
    {
        /// <summary>
        /// 虚拟目录的名字可以修改
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// 获取或设置虚拟目录的唯一标识符。
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// 获取或设置虚拟目录的依赖实现。
        /// </summary>
        string BaseOn { get; }
    }
}