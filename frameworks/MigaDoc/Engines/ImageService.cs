using Acorisoft.Miga.Doc.IO;


namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ImageService : DirectoryService
    {
        private const string SubFolders = "Images";

        public ImageService() : base(SubFolders)
        {
        }

        public ImageService(string dir) : base(SubFolders)
        {
        }

        protected internal sealed override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            base.OnRepositoryOpening(context, property);
        }

        public string GetImageFileName(Resource resource)
        {
            if (string.IsNullOrEmpty(Directory)) return string.Empty;
            return resource is null ? string.Empty : Path.Combine(Directory, resource.Location);
        }

        public Task<Resource> DirectCopyAsync(string fileName)
        {
            return Task.Run(() => DirectCopy(fileName));
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

            if (File.Exists(dst) && new FileInfo(dst).Length == info.Length)
            {
                dst = Path.Combine(Directory, ShortGuidString.GetId() + info.Extension);
                res = new Resource
                {
                    Kind     = ResourceKind.Database,
                    Location = info.Name,
                };
            }
                
            File.Copy(fileName, dst, true);

            return res;
        }

        public Stream DirectOpen(Resource resource)
        {
            if (resource.Kind != ResourceKind.Database)
            {
                return null;
            }

            var dst = GetImageFileName(resource);

            if (!File.Exists(dst))
            {
                return null;
            }

            try
            {
                using var fs = new FileStream(dst, FileMode.Open);
                var capacity = (int)Math.Pow(2, Math.Log2(fs.Length));
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