namespace Acorisoft.Miga.Doc.Labels
{
    public class Label : IObjectLabel
    {
        
        
        public string Id { get; init; }
        public string Owner { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
    }
}