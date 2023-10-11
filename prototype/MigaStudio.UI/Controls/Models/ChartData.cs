using System.Collections.Generic;

namespace  Acorisoft.FutureGL.MigaStudio.Controls.Models
{
    public class ChartData : ObservableObject 
    {
        private string _color;

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Values"/> 属性。
        /// </summary>
        public double[] Values { get; set; }
        
        public double Maximum { get; set; }
    }
    
    public sealed class ChartDataSet : List<ChartData>{}
}