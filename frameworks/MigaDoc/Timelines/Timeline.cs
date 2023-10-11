using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Entities.Timelines
{
    public class Timeline
    {
        /// <summary>
        /// 获取或设置 <see cref="Time"/> 属性。
        /// </summary>
        public string Time { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Index"/> 属性。
        /// </summary>
        public int Index{ get; set; }
        
        /// <summary>
        /// 关联的角色
        /// </summary>
        public ObservableCollection<DocumentIndex> Characters { get; set; }
        
        /// <summary>
        /// 关联的地图
        /// </summary>
        public ObservableCollection<DocumentIndex> Maps { get; set; }
        
        /// <summary>
        /// 关联的势力
        /// </summary>
        public ObservableCollection<DocumentIndex> Forces { get; set; }
    }
}