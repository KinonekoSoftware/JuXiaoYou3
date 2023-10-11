namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface ITextResourceFactory
    {
        /// <summary>
        /// 获得本地化节点
        /// </summary>
        /// <param name="instance">要获取的实例</param>
        /// <returns>返回本地化节点</returns>
        ITextResourceAdapter Adapt(FrameworkElement instance);
    }
}