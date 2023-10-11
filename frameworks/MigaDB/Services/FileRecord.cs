using Acorisoft.FutureGL.MigaDB.IO;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public class FileRecord : StorageObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string Uri { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public ResourceType Type { get; init; }
        
        public int Width { get; init; }
        
        public int Height { get; init; }
    }
}