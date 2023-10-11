namespace Acorisoft.FutureGL.MigaDB.Models
{
    public class DatabaseVersion
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime TimeOfCreated { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime TimeOfModified { get; set; }
        
        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }
    }
}