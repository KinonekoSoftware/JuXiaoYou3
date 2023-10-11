namespace Acorisoft.FutureGL.MigaDB.Core
{
    public enum DatabaseMode
    {
        /// <summary>
        /// 实际模式，打开实际文件并读写
        /// </summary>
        Release,
        
        /// <summary>
        /// 调试模式，不打开实际文件
        /// </summary>
        Debug,
        
        /// <summary>
        /// 附加模式，打开实际文件，只读不写
        /// </summary>
        Attached,
        
        /// <summary>
        /// 影子模式，打开实际文件的副本
        /// </summary>
        Shadow
    }
}