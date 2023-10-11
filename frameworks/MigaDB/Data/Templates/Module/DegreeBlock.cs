namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示程度内容块。
    /// </summary>
    public interface IDegreeBlock : IModuleBlock, IModuleBlock<int>, IHeaderlessBlock
    {
        /// <summary>
        /// 分割线
        /// </summary>
        int DivideLine { get; }
        
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; }
        
        /// <summary>
        /// 最大值
        /// </summary>
        int Maximum { get; }

        /// <summary>
        /// 最小值
        /// </summary>
        int Minimum { get; }
    }

    /// <summary>
    /// 表示程度内容块。
    /// </summary>
    public interface IDegreeBlockDataUI : IDegreeBlock, IModuleBlockDataUI, IHeaderlessBlock
    {
    }

    /// <summary>
    /// 表示程度内容块。
    /// </summary>
    public interface IDegreeBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<int>, IHeaderlessBlock
    {
        /// <summary>
        /// 分割线
        /// </summary>
        int DivideLine { get; set; }
        
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; set; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; set; }
        
        /// <summary>
        /// 最大值
        /// </summary>
        int Maximum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        int Minimum { get; set; }
    }
    
    /// <summary>
    /// 表示程度内容块。
    /// </summary>
    public class DegreeBlock : ModuleBlock, IDegreeBlock, IMetadataNumericSource
    {
        public override string GetLanguageId() => IdOfDegree;
        public int GetValue() => Value;
        
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (DegreeBlock)block;
            return Fallback == bb.Fallback &&
                   Negative == bb.Negative &&
                   Positive == bb.Positive && 
                   DivideLine == bb.DivideLine &&
                   Maximum == bb.Maximum && 
                   Minimum == bb.Minimum;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (DegreeBlock)block;
            return bb.Value == Value;
        }
        
        

        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is DegreeBlock nb && CompareTemplateOverride(nb))
            {
                nb.Value = Value;
                return true;
            }

            return false;
        }
        
        
        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value.ToString(),
                Type  = MetadataKind.Degree,
            };
        }
        public sealed override MetadataKind? ExtractType => MetadataKind.Degree;
        
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = -1;
        
        /// <summary>
        /// 当前值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public int Fallback { get; init; }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; init; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; init; }

        /// <summary>
        /// 后缀
        /// </summary>
        public int DivideLine { get; init; }
        
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Negative { get; init; }
        
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Positive { get; init; }
    }
}