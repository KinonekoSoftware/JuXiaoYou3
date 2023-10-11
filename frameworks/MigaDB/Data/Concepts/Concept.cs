namespace Acorisoft.FutureGL.MigaDB.Data.Concepts
{
    /// <summary>
    /// 表示一个概念
    /// </summary>
    /// <remarks>
    /// <see cref="Concept"/> 概念是一个抽象的概念，应该由 IConceptNavigator 实现GetDocumentById
    /// </remarks>
    public class Concept : StorageUIObject
    {
        private string _name;
        
        /// <summary>
        /// 处理器
        /// </summary>
        public KnowledgeHandler Handler { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}