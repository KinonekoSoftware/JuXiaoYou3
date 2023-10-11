namespace Acorisoft.FutureGL.MigaDB.Documents
{
    /// <summary>
    /// 表示一个文档
    /// </summary>
    public class Document : StorageObject, IMetadataPackage
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
        /// 类型
        /// </summary>
        public DocumentType Type { get; init; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 是否可删除
        /// </summary>
        public bool Removable { get; set; }

        /// <summary>
        /// 部件
        /// </summary>
        public DataPartCollection Parts { get; init; }
        
        /// <summary>
        /// 父级
        /// </summary>
        [BsonField(Constants.LiteDB_ParentId)]
        public string Owner { get; set; }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public MetadataCollection Metas { get; init; }
    }
}