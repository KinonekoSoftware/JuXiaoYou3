using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示图表内容块。
    /// </summary>
    public interface IChartBlock : IModuleBlock, IModuleBlock<int[]>
    {
        /// <summary>
        /// 最大值
        /// </summary>
        int Maximum { get; }

        /// <summary>
        /// 最小值
        /// </summary>
        int Minimum { get; }
        
        /// <summary>
        /// 轴
        /// </summary>
        string[] Axis { get; }
        
        /// <summary>
        /// 颜色
        /// </summary>
        string Color { get; }
    }

    /// <summary>
    /// 表示图表内容块。
    /// </summary>
    public interface IChartBlockDataUI : IModuleBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示图表内容块。
    /// </summary>
    public interface IChartBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<int[]>
    {
        /// <summary>
        /// 轴
        /// </summary>
        ObservableCollection<string> Axis { get; }
        
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        
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
    /// 表示图表内容块。
    /// </summary>
    public abstract class ChartBlock : ModuleBlock, IChartBlock
    {
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (ChartBlock)block;
            return Fallback == bb.Fallback &&
                   Axis.SequenceEqual(bb.Axis) &&
                   Fallback.SequenceEqual(bb.Fallback)&& 
                   Color == bb.Color &&
                   Maximum == bb.Maximum && 
                   Minimum == bb.Minimum;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (ChartBlock)block;
            return Value.SequenceEqual(bb.Value);
        }
        
        

        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is ChartBlock nb && CompareTemplateOverride(nb))
            {
                nb.Value = Value;
                return true;
            }

            return false;
        }
        
        public abstract ChartType ChartType { get; }
        
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = null;

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; init; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; init; }
        
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; init; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public int[] Value { get; set; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public int[] Fallback { get; init; }
        
        /// <summary>
        /// 轴
        /// </summary>
        public string[] Axis { get; init; }
    }

    /// <summary>
    /// 表示雷达图内容块。
    /// </summary>
    public sealed class RadarBlock : ChartBlock
    {
        public override string GetLanguageId() => IdOfRadar;
        public override Metadata ExtractMetadata()
        {
            var sb = Pool.GetStringBuilder();
            
            
            return new Metadata
            {
                Name       = Metadata,
                Value      = string.Empty,
                Type       = MetadataKind.RadarChart,
                Parameters = MetadataProcessor.ChartBaseFormatted(Axis, Value, Fallback, Maximum, Minimum, Color)
            };
        }

        public override ChartType ChartType => ChartType.Radar;
        public sealed override MetadataKind? ExtractType => MetadataKind.RadarChart;
    }

    /// <summary>
    /// 表示柱形图内容块。
    /// </summary>
    public sealed class HistogramBlock : ChartBlock
    {
        public override string GetLanguageId() => IdOfHistogram;
        public override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name       = Metadata,
                Value      = Value?.ToString(),
                Type       = MetadataKind.HistogramChart,
                Parameters = MetadataProcessor.ChartBaseFormatted(Axis, Value, Fallback, Maximum, Minimum, Color)
            };
        }
        
        public sealed override MetadataKind? ExtractType => MetadataKind.HistogramChart;
        public override ChartType ChartType => ChartType.Histogram;
    }
}