using System.Collections.Concurrent;

namespace Acorisoft.FutureGL.Forest.Styles
{
    public abstract class ForestThemeSystem : ForestObject
    {
        protected ForestThemeSystem()
        {
            Colors     = new ConcurrentDictionary<int, Color>();
            Geometries = new ConcurrentDictionary<int, Geometry>();
            Images     = new ConcurrentDictionary<int, ImageSource>();
            CornerRadius = new Bag<Thickness>
            {
                Samll  = new Thickness(8),
                Medium = new Thickness(12),
                Large  = new Thickness(24)
            };

            BorderThickness = new Bag<Thickness>
            {
                Samll  = new Thickness(2),
                Medium = new Thickness(4),
                Large  = new Thickness(8)
            };

            Duration = new Bag<Duration>
            {
                Samll  = new Duration(TimeSpan.FromMilliseconds(150)),
                Medium = new Duration(TimeSpan.FromMilliseconds(200)),
                Large  = new Duration(TimeSpan.FromMilliseconds(300)),
            };
            
        }

        public void Initialize()
        {
            InitializeColors();
        }

        private void InitializeColors()
        {
            InitializeOverlayBrush(Colors);
            InitializeBackgroundBrush(Colors);
            InitializeForegroundBrush(Colors);
            InitializeAdaptiveBrush(Colors);
            InitializeNotificationBrush(Colors);
            InitializeHighlightBrush(Colors);
        }

        /// <summary>
        /// 初始化遮罩层
        /// </summary>
        /// <param name="colors">容器</param>
        protected abstract void InitializeOverlayBrush(ConcurrentDictionary<int, Color> colors);

        /// <summary>
        /// 初始化提示色
        /// </summary>
        /// <param name="colors">容器</param>
        protected virtual void InitializeNotificationBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.Danger100, Color.FromRgb(0xbb, 0x21, 0x24));
            colors.TryAdd((int)ForestTheme.Danger200, Color.FromRgb(0xd9, 0x08, 0x0c));
            colors.TryAdd((int)ForestTheme.Danger300, Color.FromRgb(0xA6, 0x06, 0x09));
            colors.TryAdd((int)ForestTheme.Danger400, Color.FromRgb(0xec, 0x93, 0x94));
            
            colors.TryAdd((int)ForestTheme.Warning100, Color.FromRgb(0xdb, 0x94, 0x00));
            colors.TryAdd((int)ForestTheme.Warning200, Color.FromRgb(0xff, 0xb3, 0x14));
            colors.TryAdd((int)ForestTheme.Warning300, Color.FromRgb(0xa8, 0x72, 0x00));
            colors.TryAdd((int)ForestTheme.Warning400, Color.FromRgb(0xff, 0xd6, 0x80));
            
            colors.TryAdd((int)ForestTheme.Info100, Color.FromRgb(0x00, 0x7a, 0xcc));
            colors.TryAdd((int)ForestTheme.Info200, Color.FromRgb(0x00, 0x92, 0xf2));
            colors.TryAdd((int)ForestTheme.Info300, Color.FromRgb(0x00, 0x55, 0x8f));
            colors.TryAdd((int)ForestTheme.Info400, Color.FromRgb(0x80, 0xcc, 0xff));
            
            colors.TryAdd((int)ForestTheme.Success100, Color.FromRgb(0x99, 0xb4, 0x33));
            colors.TryAdd((int)ForestTheme.Success200, Color.FromRgb(0xb2, 0xcc, 0x4c));
            colors.TryAdd((int)ForestTheme.Success300, Color.FromRgb(0x76, 0x8b, 0x27));
            colors.TryAdd((int)ForestTheme.Success400, Color.FromRgb(0xdd, 0xe8, 0xb0));
            
            colors.TryAdd((int)ForestTheme.Obsolete100, Color.FromRgb(0xca, 0x51, 0x00));
            colors.TryAdd((int)ForestTheme.Obsolete200, Color.FromRgb(0xff, 0x66, 0x00));
            colors.TryAdd((int)ForestTheme.Obsolete300, Color.FromRgb(0x99, 0x3d, 0x00));
            colors.TryAdd((int)ForestTheme.Obsolete400, Color.FromRgb(0xff, 0x94, 0x4d));
            
            
            colors.TryAdd((int)ForestTheme.SlateGray100, Color.FromRgb(0x70, 0x80, 0x90));
            colors.TryAdd((int)ForestTheme.SlateGray200, Color.FromRgb(0x4f, 0x5b, 0x66));
            colors.TryAdd((int)ForestTheme.SlateGray300, Color.FromRgb(0x28, 0x2e, 0x34));
            colors.TryAdd((int)ForestTheme.SlateGray400, Color.FromRgb(0x9b, 0xa6, 0xb1));
        }
        
        /// <summary>
        /// 初始化高亮色
        /// </summary>
        /// <param name="colors">容器</param>
        protected abstract void InitializeBackgroundBrush(ConcurrentDictionary<int, Color> colors);

        /// <summary>
        /// 初始化背景色
        /// </summary>
        /// <param name="colors">容器</param>
        protected virtual void InitializeHighlightBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.HighlightA1, Color.FromRgb(0xe5, 0xe9, 0xaf));
            colors.TryAdd((int)ForestTheme.HighlightA2, Color.FromRgb(0xd1, 0xd9, 0x72));
            colors.TryAdd((int)ForestTheme.HighlightA3, Color.FromRgb(0xbe, 0xc9, 0x36));
            colors.TryAdd((int)ForestTheme.HighlightA4, Color.FromRgb(0x98, 0xa1, 0x2b));
            colors.TryAdd((int)ForestTheme.HighlightA5, Color.FromRgb(0x72, 0x79, 0x20));
            
            colors.TryAdd((int)ForestTheme.HighlightB1, Color.FromRgb(0xff, 0xda, 0x96));
            colors.TryAdd((int)ForestTheme.HighlightB2, Color.FromRgb(0xff, 0xc2, 0x54));
            colors.TryAdd((int)ForestTheme.HighlightB3, Color.FromRgb(0xff, 0xab, 0x12));
            colors.TryAdd((int)ForestTheme.HighlightB4, Color.FromRgb(0xd4, 0x89, 0x00));
            colors.TryAdd((int)ForestTheme.HighlightB5, Color.FromRgb(0x96, 0x61, 0x00));

            colors.TryAdd((int)ForestTheme.HighlightC1, Color.FromRgb(0xbf, 0xb8, 0xe9));
            colors.TryAdd((int)ForestTheme.HighlightC2, Color.FromRgb(0x95, 0x89, 0xdb));
            colors.TryAdd((int)ForestTheme.HighlightC3, Color.FromRgb(0x6a, 0x5a, 0xcd));
            colors.TryAdd((int)ForestTheme.HighlightC4, Color.FromRgb(0x48, 0x36, 0xb3));
            colors.TryAdd((int)ForestTheme.HighlightC5, Color.FromRgb(0x35, 0x28, 0x84));
        }
        
        /// <summary>
        /// 初始化前景色
        /// </summary>
        /// <param name="colors">容器</param>
        protected abstract void InitializeForegroundBrush(ConcurrentDictionary<int, Color> colors);
        
        /// <summary>
        /// 初始化适应色
        /// </summary>
        /// <param name="colors">容器</param>
        protected abstract void InitializeAdaptiveBrush(ConcurrentDictionary<int, Color> colors);

        /// <summary>
        /// 初始化其他色
        /// </summary>
        /// <param name="colors">容器</param>
        protected virtual void InitializeOthersBrush(ConcurrentDictionary<int, Color> colors)
        {
            
        }
        
        

        public Color GetCallToActionColor(HighlightColorPalette palette, int level)
        {
            return level switch
            {
                2 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB4],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC4],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger200],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning200],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info200],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success200],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete200],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray200],
                    _                                       => Colors[(int)ForestTheme.HighlightA4],
                },
                3 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB5],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC5],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger300],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning300],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info300],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success300],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete300],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray300],
                    _                                       => Colors[(int)ForestTheme.HighlightA5],
                },
                4 => palette switch
                {
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger400],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning400],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info400],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success400],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete400],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray400],
                    _                                       => Colors[(int)ForestTheme.BackgroundLevel3],
                },
                _ => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB3],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC3],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger100],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning100],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info100],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success100],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete100],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray100],
                    _                                       => Colors[(int)ForestTheme.HighlightA3],
                }
            };
        }

        public Color GetHighlightColor(HighlightColorPalette palette, int level)
        {
            return level switch
            {
                2 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB2],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC2],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger200],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning200],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info200],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success200],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete200],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray200],
                    _                                       => Colors[(int)ForestTheme.HighlightA2],
                },
                3 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB3],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC3],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger100],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning100],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info100],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success100],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete100],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray100],
                    _                                       => Colors[(int)ForestTheme.HighlightA3],
                },
                4 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB4],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC4],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger300],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning300],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info300],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success300],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete300],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray300],
                    _                                       => Colors[(int)ForestTheme.HighlightA4],
                },
                5 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB5],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC5],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger300],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning300],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info300],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success300],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete300],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray300],
                    _                                       => Colors[(int)ForestTheme.HighlightA5],
                },
                _ => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB1],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC1],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger400],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning400],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info400],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success400],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete400],
                    HighlightColorPalette.SlateGray         => Colors[(int)ForestTheme.SlateGray400],
                    _                                       => Colors[(int)ForestTheme.HighlightA1],
                }
            };
        }

        public Bag<Duration> Duration { get; }

        /// <summary>
        /// 边框的定义
        /// </summary>
        public Bag<Thickness> BorderThickness { get; }

        /// <summary>
        /// 圆角的定义
        /// </summary>
        public Bag<Thickness> CornerRadius { get; }

        /// <summary>
        /// 颜色池
        /// </summary>
        public ConcurrentDictionary<int, Color> Colors { get; }

        /// <summary>
        /// 形状池
        /// </summary>
        public ConcurrentDictionary<int, Geometry> Geometries { get; }

        /// <summary>
        /// 图形池
        /// </summary>
        public ConcurrentDictionary<int, ImageSource> Images { get; }
    }
}