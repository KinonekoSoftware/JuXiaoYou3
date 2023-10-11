namespace Acorisoft.FutureGL.MigaDB.Core
{
    public interface IDatabaseSynchronizer
    {
        /// <summary>
        /// 手动操作同步器，仅用于框架代码
        /// </summary>
        void Manual();
        
        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
        
        /// <summary>
        /// 设置
        /// </summary>
        void Set();
        
        /// <summary>
        /// 取消设置
        /// </summary>
        void Unset();
    }
}