using System.Collections.ObjectModel;

namespace  Acorisoft.FutureGL.MigaStudio.Controls.Models
{
    public class Axis : ObservableObject
    {
        private string _name;
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
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
    
    public class AxisCollection : ObservableCollection<Axis>{}
}