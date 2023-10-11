namespace Acorisoft.Miga.Doc.Engines
{
    public abstract class DirectoryService :  StorageService
    {
        protected DirectoryService(string subFolder)
        {
            SubFolder = subFolder;
        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Directory     = CheckDirectory(Path.Combine(BaseDirectory, SubFolder));
        }
        
        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            BaseDirectory = null;
            Directory     = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SubFolder { get; }
        
        /// <summary>
        /// 世界观的工作目录
        /// </summary>
        public string BaseDirectory { get; private set; }
        
        /// <summary>
        /// 当前集合的工作目录
        /// </summary>
        public string Directory { get; private set; }
    }
}