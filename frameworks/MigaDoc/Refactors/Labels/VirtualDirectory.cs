namespace Acorisoft.Miga.Doc.Labels
{
    public class VirtualDirectory :  IVirtualDirectory
    {
        

        public string Id { get; init; }
        public string BaseOn { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
    }
}