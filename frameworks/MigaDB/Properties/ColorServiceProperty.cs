using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class ColorMapping : StorageUIObject
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
        
        public ObservableCollection<string> Keywords { get; init; }
    }
    
    /// <summary>
    /// <see cref="ColorServiceProperty"/> 类型表示二创的接受程度
    /// </summary>
    public class ColorServiceProperty : ObservableObject
    {
        public List<ColorMapping> Mappings { get; init; }
        public int Version { get; set; }
    }
}