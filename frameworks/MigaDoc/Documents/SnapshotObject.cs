namespace Acorisoft.Miga.Doc.Documents
{
    public abstract class SnapshotObject 
    {
        /// <summary>
        /// 获取或设置 <see cref="Id"/> 属性。
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name { get; set; }
    }
}