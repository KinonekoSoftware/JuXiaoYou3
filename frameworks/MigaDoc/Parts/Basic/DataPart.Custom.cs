namespace Acorisoft.Miga.Doc.Parts
{
    /// <summary>
    /// <see cref="CustomDataPart"/> 类型表示一个自定义部件集合。
    /// </summary>
    public class CustomDataPart : DataPart
    {
        /// <summary>
        /// 
        /// </summary>
        public List<InputProperty> Properties { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
    }
}