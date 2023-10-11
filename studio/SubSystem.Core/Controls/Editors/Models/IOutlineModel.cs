using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaStudio.Editors.Models
{
    public interface IOutlineModel
    {
        string Name { get; }
        
        /// <summary>
        /// 获得当前的大纲等级
        /// </summary>
        public int Level { get; }
        
        /// <summary>
        /// 获取子大纲
        /// </summary>
        public List<IOutlineModel> Children { get; }
    }
}