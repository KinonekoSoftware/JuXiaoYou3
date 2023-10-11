using Acorisoft.Miga.Doc.IO;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class MusicService : DirectoryService
    {
        private const string SubFolders = "Music";
        
        public MusicService() : base(SubFolders)
        {
            IsLazyMode = true;
        }

        public string GetMusicFileName(Resource resource)
        {
            return resource is null ? string.Empty : Path.Combine(Directory, resource.Location);
        }

        public Task<Resource> DirectCopyAsync(string fileName)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return null;
                }

                if (!File.Exists(fileName))
                {
                    return null;
                }

                var info = new FileInfo(fileName);
                var dst = Path.Combine(Directory, info.Name);
                var res = new Resource
                {
                    Kind     = ResourceKind.Database,
                    Location = info.Name,
                };

                if (!File.Exists(dst))
                {
                    File.Copy(fileName, dst, true);
                }

                return res;
            });
        }
        
        public Resource DirectCopy(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            if (!File.Exists(fileName))
            {
                return null;
            }

            var info = new FileInfo(fileName);
            var dst = Path.Combine(Directory, info.Name);
            var res = new Resource
            {
                Kind     = ResourceKind.Database,
                Location = info.Name,
            };

            if (!File.Exists(dst))
            {
                File.Copy(fileName, dst, true);
            }

            return res;
        }

        public void DirectRemove(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        
        public Stream DirectOpen(Resource resource)
        {
            if (resource.Kind != ResourceKind.Database)
            {
                return null;
            }

            var dst = GetMusicFileName(resource);

            if (!File.Exists(dst))
            {
                return null;
            }

            try
            {
                using var fs = new FileStream(dst, FileMode.Open);
                var capacity = (int)(Math.Log2(fs.Length) * 2);
                var ms = new MemoryStream(capacity);

                fs.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            catch
            {
                return null;
            }
        }
    }
}