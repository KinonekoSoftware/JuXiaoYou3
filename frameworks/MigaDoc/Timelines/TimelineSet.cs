namespace Acorisoft.Miga.Doc.Entities.Timelines
{
    public class TimelineSet
    {
        
        public string Id { get; init; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public List<Timeline> Timelines { get; init; }
    }
}