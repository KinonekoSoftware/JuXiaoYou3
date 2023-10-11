namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public interface IColorBlock : IModuleBlock
    {
        
    }

    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public interface IColorBlockDataUI : IColorBlock, IModuleBlockDataUI
    {
        
    }

    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public interface IColorBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<string>
    {
        
    }

    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlock : ModuleBlock, IColorBlock, IModuleBlock<string>, IMetadataTextSource
    {
        public string GetValue() => string.IsNullOrEmpty(Value) ? Fallback : Value;
        public override string GetLanguageId() => IdOfColor;
        
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (ColorBlock)block;
            return Fallback == bb.Fallback;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (ColorBlock)block;
            return bb.Value == Value;
        }
        

        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is ColorBlock nb && CompareTemplateOverride(nb))
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
                Value = Value,
                Type  = MetadataKind.Color,
            };
        }
        public sealed override MetadataKind? ExtractType => MetadataKind.Color;
        
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = string.Empty;
        
        /// <summary>
        /// 当前值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public string Fallback { get; init; }
    }
}