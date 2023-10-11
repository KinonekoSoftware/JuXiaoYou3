namespace Acorisoft.Miga.Doc.Entities.Inspirations
{
    /// <summary>
    /// <see cref="Impression"/> 类型表示印象
    /// </summary>
    public sealed class Impression : Glimpse
    {
        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content{ get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color{ get; set; }
    }
}