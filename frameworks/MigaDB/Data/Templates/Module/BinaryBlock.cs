namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{    
    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IBinaryBlock : IModuleBlock, IModuleBlock<bool>
    {
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; }
    }

    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IBinaryBlockDataUI : IBinaryBlock, IModuleBlockDataUI, IHeaderlessBlock
    {
    }

    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IBinaryBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<bool>, IHeaderlessBlock
    {
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; set; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; set; }
    }
    
    
    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public class BinaryBlock : ModuleBlock, IBinaryBlock, IHeaderlessBlock, IMetadataBooleanSource
    {
        public bool GetValue() => Value;

        public override string GetLanguageId() => IdOfBinary;

        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (BinaryBlock)block;
            return Fallback == bb.Fallback &&
                   Negative == bb.Negative &&
                   Positive == bb.Positive;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (BinaryBlock)block;
            return bb.Value == Value;
        }

        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value.ToString(),
                Type  = MetadataKind.Switch,
            };
        }

        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is BinaryBlock nb && CompareTemplateOverride(nb))
            {
                nb.Value = Value;
                return true;
            }

            return false;
        }

        public sealed override MetadataKind? ExtractType => MetadataKind.Switch;

        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = false;
        
        /// <summary>
        /// 当前值
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public bool Fallback { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Negative { get; set; }
        
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Positive { get; set; }
    }
}