namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public interface IPartOfDetail
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// 索引号 
        /// </summary>
        int Index { get; set; }
        
        /// <summary>
        /// 是否为声明
        /// </summary>
        bool IsDeclaration { get; }
        
        /// <summary>
        /// 是否可移除
        /// </summary>
        bool Removable { get; }
    }
}