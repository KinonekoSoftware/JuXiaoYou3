
using System.ComponentModel;

namespace Acorisoft.Miga.Doc.Core
{
    
    /// <summary>
    /// <see cref="StorageObject"/> 类型表示一个存储对象。
    /// </summary>
    public abstract class StorageObject
    {
        /// <summary>
        /// 唯一标识符。
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 当前对象名。
        /// </summary>
        public string Name { get; set; }
    }
}