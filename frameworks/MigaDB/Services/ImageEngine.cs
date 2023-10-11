using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public class ImageEngine : FileEngine
    {
        public ImageEngine() : base(Constants.ImageFolderName)
        {
            
        }

        public bool HasFile(string md5)
        {
            return !string.IsNullOrEmpty(md5) &&
                   Records is not null &&
                   Records.HasID(md5);
        }

        public void AddFile(FileRecord record)
        {
            if (record is null)
            {
                return;
            }

            Records.Insert(record);
        }

        public void RemoveFile(FileRecord record)
        {
            if (record is null)
            {
                return;
            }

            Records.Delete(record.Id);
        }

        protected override void OnDatabaseClosing()
        {
            Records         = null;
            SourceDirectory = null;
            base.OnDatabaseClosing();
        }

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            //
            // 检查文件夹创建状态
            base.OnDatabaseOpening(session);
            

            SourceDirectory = Path.Combine(session.RootDirectory, "Sources");
            
            if (!Directory.Exists(SourceDirectory))
            {
                Directory.CreateDirectory(SourceDirectory);
            }
            
            
            Records = session.Database
                             .GetCollection<FileRecord>(Constants.Name_FileTable);
        }

        public void WriteAvatar(MemoryStream ms, string resource)
        {
            if (ms is null || ms.Length == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(resource))
            {
                return;
            }

            var dst = Path.Combine(FullDirectory, resource);
            var fs  = new FileStream(dst, FileMode.Create, FileAccess.Write);

            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);
            fs.Dispose();
        }

        public void Write(string resource, byte[] buffer)
        {
            if (buffer is null || buffer.Length == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(resource))
            {
                return;
            }

            var dst = Path.Combine(FullDirectory, resource);
            var fs  = new FileStream(dst, FileMode.Create, FileAccess.Write);
            fs.Write(buffer);
            fs.Dispose();
        }

        public string GetSourceFileName(string id) => Path.Combine(SourceDirectory, id);
        public ILiteCollection<FileRecord> Records { get; private set; }

        public static string GetAvatarUri() => $"avatar_{ID.Get()}.png";
        
        public string SourceDirectory { get; private set; }
    }
}