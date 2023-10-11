namespace Acorisoft.FutureGL.Forest.Models
{
    public abstract class Notification : ForestObject
    {
        private string   _title;
        private Color    _color;
        private TimeSpan _delay;

        internal void Initialize() => _delay = TimeSpan.FromSeconds(1);
        
        /// <summary>
        /// 延迟
        /// </summary>
        public TimeSpan Delay { get => _delay; init => _delay = value; }

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public Color Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }

        public SolidColorBrush Brush => new SolidColorBrush(Color);
        
        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }
    }

    public sealed class IconNotification : Notification
    {
        private Geometry _geometry;
        private bool _isFilled;

        /// <summary>
        /// 获取或设置 <see cref="IsFilled"/> 属性。
        /// </summary>
        public bool IsFilled
        {
            get => _isFilled;
            set => SetValue(ref _isFilled, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Geometry"/> 属性。
        /// </summary>
        public Geometry Geometry
        {
            get => _geometry;
            set => SetValue(ref _geometry, value);
        }
    }
    
    public sealed class ImageNotification : Notification
    {
        private Geometry _imageSource;

        /// <summary>
        /// 获取或设置 <see cref="ImageSource"/> 属性。
        /// </summary>
        public Geometry ImageSource
        {
            get => _imageSource;
            set => SetValue(ref _imageSource, value);
        }
    }
}