using System.Collections.ObjectModel;


namespace Acorisoft.Miga.Doc.Keywords
{
    public class Keyword 
    {
        
        private string _summary;
        
        public string Id { get; init; }
        public string Owner { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
    }
}