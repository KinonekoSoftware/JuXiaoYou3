using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class FolderCounter : ObservableObject
    {
        private long   _size;
        private string _sizeString;
        private double _percent;
        
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 目录位置
        /// </summary>
        public string Directory { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="SizeString"/> 属性。
        /// </summary>
        public string SizeString
        {
            get => _sizeString;
            private set => SetValue(ref _sizeString, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Percent"/> 属性。
        /// </summary>
        public double Percent
        {
            get => _percent;
            set
            {
                SetValue(ref _percent, value);
                RaiseUpdated(nameof(Size));
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Size"/> 属性。
        /// </summary>
        public long Size
        {
            get => _size;
            set
            {
                _size = value;
                SizeString = IOSystemUtilities.GetFileSizeString(value);
            }
        }
    }

    public class DatabaseCounter : FolderCounter
    {
        public ObservableCollection<FolderCounter> Counters { get; init; }
    }

    public class EngineCounter : FolderCounter
    {
        
    }
}