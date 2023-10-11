using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    /// <summary>
    /// 文档缓存
    /// </summary>
    [DebuggerDisplay("{Name}-{Id}")]
    public class DocumentCache : ObservableObject, IDataCache, IEquatable<DocumentCache>
    {
        private bool   _isLocked;
        private string _avatar;
        private string _name;
        private string _intro;
        

        public bool Equals(DocumentCache other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Type == other.Type && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DocumentCache)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, (int)Type);
        }
        
        
        /// <summary>
        /// 唯一标识符
        /// </summary>
        [BsonId]
        public string Id { get; init; }
        
        /// <summary>
        /// 父级
        /// </summary>
        [BsonField(Constants.LiteDB_ParentId)]
        public string Owner { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public DocumentType Type { get; init; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }

        /// <summary>
        /// 是否可删除
        /// </summary>
        public bool Removable { get; init; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked
        {
            get => _isLocked;
            set => SetValue(ref _isLocked, value);
        }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime TimeOfCreated { get; init; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime TimeOfModified { get; set; }
        
        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
    }
}