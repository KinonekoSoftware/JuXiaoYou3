namespace Acorisoft.FutureGL.MigaDB.Services
{
    /// <summary>
    /// 取消安装
    /// </summary>
    public interface IDataEngine
    {
        /// <summary>
        /// 激活
        /// </summary>
        /// <returns></returns>
        bool Activate();
        
        /// <summary>
        /// 是否激活
        /// </summary>
        bool Activated { get; }
        
        /// <summary>
        /// 是否为懒加载模式
        /// </summary>
        bool IsLazyMode { get; set; }
    }
}