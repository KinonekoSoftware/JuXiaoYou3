namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class ModuleTemplateCache : ObservableObject
    {
        private bool _isRemovable;
        
        [BsonId]
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="IsRemovable"/> 属性。
        /// </summary>
        public bool IsRemovable
        {
            get => _isRemovable;
            set => SetValue(ref _isRemovable, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="ForType"/> 属性。
        /// </summary>
        public DocumentType ForType { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="MetadataList"/> 属性。
        /// </summary>
        public List<MetadataCache> MetadataList { get; init; }
        
        /// <summary>
        /// 获取或设置 <see cref="Version"/> 属性。
        /// </summary>
        public int Version { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="AuthorList"/> 属性。
        /// </summary>
        public string AuthorList { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name { get; set; }

        public string ContractList { get; set; }
    }
}