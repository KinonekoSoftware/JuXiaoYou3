namespace Acorisoft.Miga.Doc.Labels
{
    /// <summary>
    /// 用于实现
    /// </summary>
    public class LabelMapping : IObjectMapping
    {
        public string Id { get; init; }
        public string TargetId { get; init; }
        public string LabelId { get; init; }
        public bool IsDocument { get; init; }
        public bool IsPage { get; init; }
    }

    public enum DocumentEntity
    {
        Assets,
        Character,
        Material,
        Map,
        Skill,
        Passage
    }

    public class LabelRel
    {
        public string Id { get; init; }
        public string TargetId { get; init; }
        public string LabelId { get; init; }
        public DocumentEntity Entity { get; init; }
    }
}