using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMonoSelectorBlock : IModuleBlock, IModuleBlock<string>
    {
        /// <summary>
        /// 选项
        /// </summary>
        List<OptionItem> Items { get; }
    }

    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMonoSelectorBlockDataUI : IModuleBlock, IModuleBlock<string>, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMonoSelectorBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<string>
    {
        /// <summary>
        /// 允许值和选项不一样
        /// </summary>
        bool AllowDiffValue { get; set; }
        
        /// <summary>
        /// 选项
        /// </summary>
        ObservableCollection<OptionItem> Items { get; }
    }
    
    public abstract class MonoSelectorBlock : ModuleBlock, IMonoSelectorBlock
    {
        public override void ClearValue()
        {
            Value = null;
        }
        
        protected sealed override bool CompareTemplateOverride(ModuleBlock block)
        {
            var mono = (MonoSelectorBlock)block;
            return Fallback == mono.Fallback &&
                   Items.SequenceEqual(mono.Items);
        }

        protected sealed override bool CompareValueOverride(ModuleBlock block)
        {
            var mono = (MonoSelectorBlock)block;
            return Value == mono.Value;
        }

        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is MonoSelectorBlock nb && CompareTemplateOverride(nb))
            {
                nb.Value = Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Fallback { get; init; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 选项
        /// </summary>
        public List<OptionItem> Items { get; init; }
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed class SequenceBlock : MonoSelectorBlock, IMetadataTextSource
    {
        public override string GetLanguageId() => IdOfSequence;
        
        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value,
                Type  = MetadataKind.Text,
            };
        }

        public string GetValue() => Value;
        public sealed override MetadataKind? ExtractType => MetadataKind.Text;
    }
}