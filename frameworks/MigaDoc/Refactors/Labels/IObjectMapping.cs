namespace Acorisoft.Miga.Doc.Labels
{
    public interface IObjectMapping
    {
        /// <summary>
        /// 当前对象映射的唯一标识符。
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// 映射的目标唯一标识符
        /// </summary>
        string TargetId { get; }
        
        /// <summary>
        /// 标签的唯一标识符。
        /// </summary>
        string LabelId { get; }
    }
}