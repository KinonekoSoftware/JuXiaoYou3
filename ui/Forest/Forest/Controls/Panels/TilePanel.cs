
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Acorisoft.FutureGL.Forest.Controls.Panels
{
    // 
    // Thx: nick121212
    // https://www.cnblogs.com/nicktyui
    /// <summary>
    /// TilePanel
    /// 瀑布流布局
    /// </summary>
    public class TilePanel : Panel
    {
        #region 枚举

        private enum OccupyType
        {
            NONE,
            WIDTHHEIGHT,
            OVERFLOW
        }

        #endregion

        #region 属性
        
        /// <summary>
        /// 容器内元素的高度
        /// </summary>
        public static readonly DependencyProperty TileWidthProperty =
            DependencyProperty.Register(
                nameof(TileWidth),
                typeof(int),
                typeof(TilePanel),
                new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.AffectsMeasure));


        public static readonly DependencyProperty TileHeightProperty = DependencyProperty.Register(
            nameof(TileHeight),
            typeof(int),
            typeof(TilePanel),
            new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int TileHeight
        {
            get => (int)GetValue(TileHeightProperty);
            set => SetValue(TileHeightProperty, value);
        }
        
        /// <summary>
        /// 容器内元素的宽度
        /// </summary>
        public int TileWidth
        {
            get { return (int)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }
       
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetW(DependencyObject obj)
        {
            return (int)obj.GetValue(WProperty);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetW(DependencyObject obj, int value)
        {
            if (value > 0)
            {
                obj.SetValue(WProperty, value);
            }
        }
        /// <summary>
        /// 元素的宽度比例，相对于TileWidth
        /// </summary>
        public static readonly DependencyProperty WProperty =
            DependencyProperty.RegisterAttached(
                "W", 
                typeof(int),
                typeof(TilePanel), 
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetH(DependencyObject obj)
        {
            return (int)obj.GetValue(HProperty);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetH(DependencyObject obj, int value)
        {
            if (value > 0)
            {
                obj.SetValue(HProperty, value);
            }
        }
        /// <summary>
        /// 元素的高度比例，相对于TileHeight
        /// </summary>
        public static readonly DependencyProperty HProperty =
            DependencyProperty.RegisterAttached(
                "H", 
                typeof(int), 
                typeof(TilePanel),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        /// <summary>
        /// 排列方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        /// <summary>
        /// 排列方向
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation), 
                typeof(Orientation),
                typeof(TilePanel), 
                new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// 格子数量
        /// </summary>
        public int TileCount
        {
            get { return (int)GetValue(TileCountProperty); }
            set { SetValue(TileCountProperty, value); }
        }
        /// <summary>
        /// 格子数量
        /// </summary>
        public static readonly DependencyProperty TileCountProperty =
            DependencyProperty.Register(
                nameof(TileCount), 
                typeof(int), 
                typeof(TilePanel), 
                new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// Tile之间的间距
        /// </summary>
        public Thickness Gap
        {
            get { return (Thickness)GetValue(GapProperty); }
            set { SetValue(GapProperty, value); }
        }
        /// <summary>
        /// Tile之间的间距
        /// </summary>
        public static readonly DependencyProperty GapProperty =
            DependencyProperty.Register(nameof(Gap), 
                typeof(Thickness),
                typeof(TilePanel), 
                new FrameworkPropertyMetadata(new Thickness(2), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 最小的高度比例
        /// </summary>
        private int MinHeightPix;

        /// <summary>
        /// 最小的宽度比例
        /// </summary>
        private int MinWidthPix;

        #endregion

        #region 方法

        private Dictionary<string, Point> Maps { get; set; }
        private OccupyType SetMaps(Point currentPosition, Size childPix)
        {
            var isOccupy = OccupyType.NONE;

            if (currentPosition.X + currentPosition.Y != 0)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    isOccupy = IsOccupyWidth(currentPosition, childPix);
                }
                else
                {
                    isOccupy = IsOccupyHeight(currentPosition, childPix);
                }
            }

            if (isOccupy == OccupyType.NONE)
            {
                for (var i = 0; i < childPix.Width; i++)
                {
                    for (var j = 0; j < childPix.Height; j++)
                    {
                        Maps[$"x_{currentPosition.X + i}y_{currentPosition.Y + j}"] = new Point(currentPosition.X + i, currentPosition.Y + j);
                    }
                }
            }

            return isOccupy;
        }
        private OccupyType IsOccupyWidth(Point currentPosition, Size childPix)
        {
            //计算当前行能否放下当前元素
            if (TileCount - currentPosition.X - childPix.Width < 0)
            {
                return OccupyType.OVERFLOW;
            }

            for (var i = 0; i < childPix.Width; i++)
            {
                if (Maps.ContainsKey($"x_{currentPosition.X + i}y_{currentPosition.Y}"))
                {
                    return OccupyType.WIDTHHEIGHT;
                }
            }

            return OccupyType.NONE;
        }
        private OccupyType IsOccupyHeight(Point currentPosition, Size childPix)
        {
            //计算当前行能否放下当前元素
            if (TileCount - currentPosition.Y - childPix.Height < 0)
            {
                return OccupyType.OVERFLOW;
            }

            for (var i = 0; i < childPix.Height; i++)
            {
                if (Maps.ContainsKey($"x_{currentPosition.X}y_{currentPosition.Y + i}"))
                {
                    return OccupyType.WIDTHHEIGHT;
                }
            }

            return OccupyType.NONE;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var childPix = new Size();
            var childPosition = new Point();
            Point? lastChildPosition = null;

            Maps = new Dictionary<string, Point>();
            for (var i = 0; i < Children.Count; )
            {
                var child = Children[i] as FrameworkElement;
                
                if(child is null)
                {
                    continue;
                }
                
                childPix.Width = GetW(child);
                childPix.Height = GetH(child);

                if (Orientation == Orientation.Vertical)
                {
                    if (childPix.Height > TileCount)
                    {
                        childPix.Height = TileCount;
                    }
                }
                else
                {
                    if (childPix.Width > TileCount)
                    {
                        childPix.Width = TileCount;
                    }
                }
                var isOccupy = SetMaps(childPosition, childPix);
                //换列
                if (isOccupy == OccupyType.WIDTHHEIGHT)
                {
                    if (lastChildPosition == null) lastChildPosition = childPosition;
                    if (Orientation == Orientation.Horizontal)
                    {
                        childPosition.X += MinWidthPix;
                    }
                    else
                    {
                        childPosition.Y += MinHeightPix;
                    }
                }
                //换行
                else if (isOccupy == OccupyType.OVERFLOW)
                {
                    lastChildPosition ??= childPosition;
                    if (Orientation == Orientation.Horizontal)
                    {
                        childPosition.X = 0;
                        childPosition.Y += Maps[$"x_{childPosition.X}y_{childPosition.Y}"].Y;
                        //childPosition.Y++;//= this.MinHeightPix;
                    }
                    else
                    {
                        childPosition.Y = 0;
                        childPosition.X += Maps[$"x_{childPosition.X}y_{childPosition.Y}"].X;
                        //childPosition.X++;//= this.MinWidthPix;
                    }
                }
                else
                {
                    i++;
                    child.Arrange(new Rect(childPosition.X * TileWidth + Math.Floor(childPosition.X / MinWidthPix) * (Gap.Left + Gap.Right),
                                           childPosition.Y * TileHeight + Math.Floor(childPosition.Y / MinHeightPix) * (Gap.Top + Gap.Bottom),
                                           child.DesiredSize.Width, child.DesiredSize.Height));
                    if (lastChildPosition != null)
                    {
                        childPosition = (Point)lastChildPosition;
                        lastChildPosition = null;
                    }
                    else
                    {
                        if (Orientation == Orientation.Horizontal)
                        {
                            childPosition.X += childPix.Width;
                            if (childPosition.X == TileCount)
                            {
                                childPosition.X = 0;
                                childPosition.Y++;
                            }
                        }
                        else
                        {
                            childPosition.Y += childPix.Height;
                            if (childPosition.Y == TileCount)
                            {
                                childPosition.Y = 0;
                                childPosition.X++;
                            }
                        }
                    }
                }
            }

            return finalSize;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="Document"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size Document)
        {
            int childWidthPix, childHeightPix, maxRowCount = 0;

            if (Children.Count == 0) return new Size();
            //遍历孩子元素
            foreach (FrameworkElement child in Children)
            {
                childWidthPix = GetW(child);
                childHeightPix = GetH(child);

                if (MinHeightPix == 0) MinHeightPix = childHeightPix;
                if (MinWidthPix == 0) MinWidthPix = childWidthPix;

                if (MinHeightPix > childHeightPix) MinHeightPix = childHeightPix;
                if (MinWidthPix > childWidthPix) MinWidthPix = childWidthPix;
            }

            foreach (FrameworkElement child in Children)
            {
                childWidthPix = GetW(child);
                childHeightPix = GetH(child);

                child.Margin = Gap;
                child.HorizontalAlignment = HorizontalAlignment.Left;
                child.VerticalAlignment = VerticalAlignment.Top;

                child.Width = TileWidth * childWidthPix + (child.Margin.Left + child.Margin.Right) *
                    ((childWidthPix - MinWidthPix) / MinWidthPix);
                child.Height = TileHeight * childHeightPix + (child.Margin.Top + child.Margin.Bottom) * ((childHeightPix - MinHeightPix) / MinHeightPix);

                maxRowCount += childWidthPix * childHeightPix;

                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            if (TileCount <= 0) throw new ArgumentOutOfRangeException();
            //if (this.MinWidthPix == 0) this.MinWidthPix = 1;
            //if (this.MinHeightPix == 0) this.MinHeightPix = 1;
            if (Orientation == Orientation.Horizontal)
            {
                Width = Document.Width = TileCount * TileWidth + TileCount / MinWidthPix * (Gap.Left + Gap.Right);
                var heightPix = Math.Ceiling((double)maxRowCount / TileCount);
                if (!double.IsNaN(heightPix))
                    Document.Height = heightPix * TileHeight + heightPix / MinHeightPix * (Gap.Top + Gap.Bottom);
            }
            else
            {
                Height = Document.Height = TileCount * TileHeight + TileCount / MinHeightPix * (Gap.Top + Gap.Bottom);
                var widthPix               = Math.Ceiling((double)maxRowCount / TileCount);
                if (!double.IsNaN(widthPix))
                    Document.Width = widthPix * TileWidth + widthPix / MinWidthPix * (Gap.Left + Gap.Right);
            }

            return Document;
        }

        #endregion
    }
}