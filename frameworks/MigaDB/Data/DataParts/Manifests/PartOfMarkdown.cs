namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfMarkdown : PartOfManifest
    {
        public PartOfMarkdown()
        {
            Id = Constants.IdOfMarkdownPart;
        }


        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content { get; set; }
    }

    public class PartOfRtf : PartOfManifest
    {
        
        public PartOfRtf()
        {
            Id = Constants.IdOfRtfPart;
        }


        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content { get; set; }
    }
}