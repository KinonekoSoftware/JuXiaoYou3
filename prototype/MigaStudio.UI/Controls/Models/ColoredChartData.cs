using System.Collections.Generic;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Models
{
    public class ColoredChartData : ObservableObject 
    {
        private string _name;
        private string _color;
        private double _value;

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public double Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
 
    public class PieChartData : ColoredChartData{}
    public class HistogramChartData : ColoredChartData{}
    
    
    public sealed class PieChartDataSet : List<PieChartData>{}

    public sealed class HistogramChartDataSet : List<HistogramChartData>
    {
        public HistogramChartDataSet() : base()
        {
            
        }
        
        public HistogramChartDataSet(int maximum) : base()
        {
            Maximum = maximum;
        }
        
        public HistogramChartDataSet(IEnumerable<HistogramChartData> collection) : base(collection)
        {
            
        }
        
        public int Maximum { get; init; }
    }
}