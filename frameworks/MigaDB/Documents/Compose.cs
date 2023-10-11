
namespace Acorisoft.FutureGL.MigaDB.Documents
{
    /// <summary>
    /// 表示一个创作
    /// </summary>
    public class Compose : StorageObject, IMetadataPackage
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 父级
        /// </summary>
        [BsonField(Constants.LiteDB_ParentId)]
        public string Owner { get; set; }
        
        /// <summary>
        /// 部件
        /// </summary>
        public DataPartCollection Parts { get; init; }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public MetadataCollection Metas { get; init; }
    }
}