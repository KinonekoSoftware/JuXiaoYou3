namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class RepositoryCache : IEquatable<RepositoryCache>
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; init; }
        
        /// <summary>
        /// 世界观名字
        /// </summary>
        public string Name { get; set; }
        
        
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        
        
        /// <summary>
        /// 世界观简介
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; init; }
        
        /// <summary>
        /// 打开的次数
        /// </summary>
        public int OpenCount { get; set; }

        public bool Equals(RepositoryCache other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Path == other.Path;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RepositoryCache)obj);
        }

        public sealed override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Path);
        }
    }
}