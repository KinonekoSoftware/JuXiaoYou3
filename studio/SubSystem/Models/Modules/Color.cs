using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlockDataUI : ModuleBlockDataUI, IColorBlockDataUI
    {
        private const string Pattern = "R({0}) G({1}) B({2})";
        private       Color  _value;
        private       string _rgb;
        private       string _hex;

        public ColorBlockDataUI(ColorBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public ColorBlockDataUI(ColorBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            TargetBlock = block;
            Fallback    = Xaml.FromHex(block.Fallback);
            Value       = string.IsNullOrEmpty(block.Value) ? Fallback : Xaml.FromHex(block.Value);
        }

        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected ColorBlock TargetBlock { get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public Color Fallback { get; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public Color Value
        {
            get => _value;
            set
            {
                if (SetValue(ref _value, value))
                {
                    Hex               = value.ToString();
                    Rgb               = string.Format(Pattern, value.R, value.G, value.B);
                    TargetBlock.Value = Hex;
                }
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="Rgb"/> 属性。
        /// </summary>
        public string Rgb
        {
            get => _rgb;
            set => SetValue(ref _rgb, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Hex"/> 属性。
        /// </summary>
        public string Hex
        {
            get => _hex;
            set => SetValue(ref _hex, value);
        }
    }
    
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlockEditUI : ModuleBlockEditUI<ColorBlock, string>, IColorBlockEditUI
    {
        public ColorBlockEditUI(ColorBlock block) : base(block)
        {
        }

        protected override ColorBlock CreateInstanceOverride()
        {
            return new ColorBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback,
            };
        }
    }
}