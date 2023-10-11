namespace Acorisoft.Miga.Doc.Entities
{
    public class Appraise 
    {
        
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 人物
        /// </summary>
        public string Avatar { get; set; }
        
        public string Name { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content{ get; set; }
    }
}