namespace Acorisoft.Miga.Doc.Parts
{
    /// <summary>
    /// <see cref="DataPart"/> 类型表示一个部件。
    /// </summary>
    public abstract class DataPart 
    {

        public virtual void Initialized(IMetadataManager metadataManager)
        {
        }
        
        
        public int Version { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public int Priority { get; set; }
    }

    public abstract class FixedDataPart : DataPart
    {
    }
}