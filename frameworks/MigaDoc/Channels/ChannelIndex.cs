namespace Acorisoft.Miga.Doc.Channels
{
    public class ChannelIndex
    {

        public string Id { get; init; }
        public string Owner { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
    }
}