namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IOptionBlock : IModuleBlock, IModuleBlock<bool>, IHeaderlessBlock
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IOptionBlockDataUI : IOptionBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IOptionBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<bool>, IHeaderlessBlock
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public abstract class OptionBlock : ModuleBlock, IOptionBlock
    {
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (OptionBlock)block;
            return Fallback == bb.Fallback;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (OptionBlock)block;
            return bb.Value == Value;
        }
        
        

        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is OptionBlock nb && CompareTemplateOverride(nb))
            {
                nb.Value = Value;
                return true;
            }

            return false;
        }

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
        public bool Fallback { get; init; }
    }

    /// <summary>
    /// 表示开关内容块。
    /// </summary>
    public sealed class SwitchBlock : OptionBlock, IMetadataBooleanSource
    {
        public override string GetLanguageId() => IdOfSwitch;
        public bool GetValue() => Value;
        
        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value.ToString(),
                Type  = MetadataKind.Switch,
            };
        }
        public sealed override MetadataKind? ExtractType => MetadataKind.Switch;
    }

    /// <summary>
    /// 表示红心内容块。原来的Favorite
    /// </summary>
    public sealed class HeartBlock : OptionBlock, IMetadataBooleanSource
    {
        public override string GetLanguageId() => IdOfHeart;
        public bool GetValue() => Value;
        
        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value.ToString(),
                Type  = MetadataKind.Heart,
            };
        }
        public sealed override MetadataKind? ExtractType => MetadataKind.Heart;
    }


    /// <summary>
    /// 表示星星内容块。原来的Talent
    /// </summary>
    public sealed class StarBlock : OptionBlock, IMetadataBooleanSource
    {
        public override string GetLanguageId() => IdOfStar;
        public bool GetValue() => Value;
        
        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value.ToString(),
                Type  = MetadataKind.Star,
            };
        }
        public sealed override MetadataKind? ExtractType => MetadataKind.Star;
    }
}