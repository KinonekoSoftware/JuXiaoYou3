namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlock : ITemplateEquatable, IValueEquatable
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 喵喵咒语
        /// </summary>
        string Metadata { get; }

        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        string ToolTips { get; }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlock<T>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        T Fallback { get; }

        /// <summary>
        /// 当前值
        /// </summary>
        T Value { get; }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlockEditUI
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 喵喵咒语
        /// </summary>
        string Metadata { get; set; }

        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        string ToolTips { get; set; }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlockEditUI<T>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        T Fallback { get; }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlockDataUI : IModuleBlock
    {
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlock : StorageObject
    {
        public const string IdOfGroup      = "__Group";
        public const string IdOfBinary     = "__Binary";
        public const string IdOfColor      = "__Color";
        public const string IdOfRadar      = "__Radar";
        public const string IdOfHistogram  = "__Histogram";
        public const string IdOfLikability = "__Likability";
        public const string IdOfRate       = "__Rate";
        public const string IdOfDegree     = "__Degree";
        public const string IdOfHybridSlider     = "__HybridSlider";
        public const string IdOfSequence   = "__Sequence";
        public const string IdOfMultiLine  = "__MultiLine";
        public const string IdOfSingleLine = "__SingleLine";
        public const string IdOfSlider     = "__Slider";
        public const string IdOfSwitch     = "__Switch";
        public const string IdOfHeart      = "__Heart";
        public const string IdOfStar       = "__Star";
        public const string IdOfNumber     = "__Number";
        public const string IdOfAudio      = "__Audio";
        public const string IdOfFile       = "__File";
        public const string IdOfMusic      = "__Music";
        public const string IdOfImage      = "__Image";
        public const string IdOfVideo      = "__Video";
        public const string IdOfReference  = "__Reference";

        internal string _name;

        public bool CompareTemplate(ModuleBlock block)
        {
            return block is not null &&
                   Id == block.Id &&
                   Name == block.Name &&
                   IsEmptyOrEquals(Metadata, block.Metadata) &&
                   CompareTemplateOverride(block);
        }


        protected abstract bool CompareTemplateOverride(ModuleBlock block);
        protected abstract bool CompareValueOverride(ModuleBlock block);
        public abstract string GetLanguageId();

        protected static bool IsEmptyOrEquals(string AValue, string BValue)
        {
            if (string.IsNullOrEmpty(AValue))
            {
                return true;
            }

            return AValue == BValue;
        }

        public bool CompareValue(ModuleBlock block)
        {
            return CompareTemplate(block) && CompareValueOverride(block);
        }

        /// <summary>
        /// 提交元数据
        /// </summary>
        /// <returns>返回一个元数据。</returns>
        public abstract Metadata ExtractMetadata();

        /// <summary>
        /// 复制值（要求为全等复制）
        /// </summary>
        public abstract bool CopyTo(ModuleBlock newBlock);

        /// <summary>
        /// 清除当前值。
        /// </summary>
        public abstract void ClearValue();

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get => _name; init => _name = value; }

        /// <summary>
        /// 喵喵咒语
        /// </summary>
        public string Metadata { get; init; }

        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        public string ToolTips { get; set; }

        public abstract MetadataKind? ExtractType { get; }

        public sealed override string ToString()
        {
            return Name;
        }
    }
}