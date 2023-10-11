namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示滑块内容块。
    /// </summary>
    public class SliderBlock : ModuleBlock, INumberBlock, IHeaderlessBlock, IMetadataNumericSource
    {
        public override string GetLanguageId() => IdOfSlider;
        public int GetValue() => Value;
        
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (SliderBlock)block;
            return Fallback == bb.Fallback &&
                   Suffix == bb.Suffix &&
                   Maximum == bb.Maximum && 
                   Minimum == bb.Minimum;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (SliderBlock)block;
            return bb.Value == Value;
        }
        
        
        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is SliderBlock nb && CompareTemplateOverride(nb))
            {
                nb.Value = Value;
                return true;
            }

            return false;
        }
        
        public sealed override Metadata ExtractMetadata()
        {
            var value = Value == -1 ? Fallback : Value;
            return new Metadata
            {
                Name       = Metadata,
                Value      = Value.ToString(),
                Type       = MetadataKind.Progress,
                Parameters = MetadataProcessor.NumberBaseFormatted(Maximum, Minimum, value)
            };
        }
        
        public sealed override MetadataKind? ExtractType => MetadataKind.Progress;
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
        public int Minimum { get;init; }
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get;init; }
    }
}