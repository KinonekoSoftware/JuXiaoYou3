using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public class Music : ObservableObject
    {
        private string _name;
        private string _author;
        private string _cover;
        
        /// <summary>
        /// 唯一标识符
        /// </summary>
        [BsonId]
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Cover"/> 属性。
        /// </summary>
        public string Cover
        {
            get => _cover;
            set => SetValue(ref _cover, value);
        }
        
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Author"/> 属性。
        /// </summary>
        public string Author
        {
            get => _author;
            set => SetValue(ref _author, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
    
    public class MusicEngine : FileEngine
    {
        public MusicEngine() : base("Musics")
        {
            
        }
        
        
        
        public bool HasFile(string id)
        {
            return !string.IsNullOrEmpty(id) &&
                   DB is not null &&
                   DB.HasID(id);
        }
        

        public Music GetFile(string fileName) => DB.FindById(fileName);
        
        public void AddFile(Music record)
        {
            if (record is null)
            {
                return;
            }

            DB.Insert(record);
        }
        
        public void RemoveFile(Music record)
        {
            if (record is null)
            {
                return;
            }

            DB.Delete(record.Id);
        }

        protected override void OnDatabaseClosing()
        {
            DB = null;
        }

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            DB = session.Database.GetCollection<Music>(Constants.Name_MusicTable);
            base.OnDatabaseOpening(session);
        }

        public async Task WriteAlbum(byte[] buffer, string resource)
        {
            if (buffer is null || buffer.Length == 0)
            {
                return;
            }

            if (resource is null)
            {
                return;
            }

            if (File.Exists(resource))
            {
                return;
            }

            var dst = Path.Combine(FullDirectory, resource);
            var fs  = new FileStream(dst, FileMode.Create, FileAccess.Write);
            await fs.WriteAsync(buffer);
            await fs.DisposeAsync();
        }
        
        public ILiteCollection<Music> DB { get; private set; }
    }
}