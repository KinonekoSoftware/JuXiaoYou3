namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    /// <summary>
    /// 这是模组的部件
    /// </summary>
    public class PartOfModule : DataPart
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 顺序
        /// </summary>
        public int Index { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Version { get; init; }
        
        /// <summary>
        /// 内容块
        /// </summary>
        public List<ModuleBlock> Blocks { get; init; }
    }
}