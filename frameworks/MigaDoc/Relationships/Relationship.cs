// ReSharper disable ConstantConditionalAccessQualifier
// ReSharper disable NotNullMemberIsNotInitialized

namespace Acorisoft.Miga.Doc.Relationships
{
    public class Relationship
    {
        public string Id { get; init; }
        
        public DocumentIndex Source { get; set; }
        
        public DocumentIndex Target { get; set; }
        
        public bool IsBidirection { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
    }

    public class RelCopy
    {
        
        public string Id { get; init; }
        public string Target { get; init; }
        public string Avatar { get; init; }
        public string Name { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="RelationName"/> 属性。
        /// </summary>
        public string RelationName{ get; set; }

    }
}