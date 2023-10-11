namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    public class Keyword : StorageObject
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 文档ID
        /// </summary>
        public string DocumentId { get; init; }
    }
}