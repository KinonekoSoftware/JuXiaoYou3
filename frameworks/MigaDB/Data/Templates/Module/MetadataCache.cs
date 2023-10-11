namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class MetadataCache : StorageObject
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 名字
        /// </summary>
        public string MetadataName { get; init; }
        
        /// <summary>
        /// 引用数量
        /// </summary>
        public int RefCount { get; set; }
    }
}