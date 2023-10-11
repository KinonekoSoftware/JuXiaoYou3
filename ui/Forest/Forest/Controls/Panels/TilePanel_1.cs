namespace Acorisoft.FutureGL.Forest.Controls.Panels
{
    // public class TilePanel : Panel
    // {
    //     #region Attach Properties
    //
    //     public static readonly DependencyProperty WProperty = DependencyProperty.RegisterAttached(
    //         "WOverride", typeof(int), typeof(TilePanel), new PropertyMetadata(Boxing.IntValues[1]));
    //
    //     public static readonly DependencyProperty HProperty = DependencyProperty.RegisterAttached(
    //         "HOverride", typeof(int), typeof(TilePanel), new PropertyMetadata(Boxing.IntValues[1]));
    //
    //     public static void SetH(DependencyObject element, int value)
    //     {
    //         element.SetValue(HProperty, Math.Clamp(value, 1, 64));
    //     }
    //
    //     public static int GetH(DependencyObject element)
    //     {
    //         return (int)element.GetValue(HProperty);
    //     }
    //
    //     public static void SetW(DependencyObject element, int value)
    //     {
    //         element.SetValue(WProperty, Math.Clamp(value, 1, 64));
    //     }
    //
    //     public static int GetW(DependencyObject element)
    //     {
    //         return (int)element.GetValue(WProperty);
    //     }
    //
    //     #endregion
    //
    //     public static readonly DependencyProperty ItemSizeProperty = DependencyProperty.Register(
    //         nameof(ItemSize),
    //         typeof(double),
    //         typeof(TilePanel),
    //         new PropertyMetadata(32d));
    //
    //     public static readonly DependencyProperty GapProperty = DependencyProperty.Register(
    //         nameof(Gap),
    //         typeof(double),
    //         typeof(TilePanel),
    //         new PropertyMetadata(default(double)));
    //
    //     private class ArrangeItem
    //     {
    //         public int              w;
    //         public int              h;
    //         public FrameworkElement element;
    //     }
    //
    //     private class LayoutWindow
    //     {
    //         private double pixelX;
    //         private double pixelY;
    //         private int    gapH;
    //         private int    gapW;
    //         private int    gapX;
    //         private int    gapY;
    //         private double size;
    //         private double gap;
    //
    //         private int maxW;
    //         private int maxH;
    //         private int w;
    //         private int h;
    //         private int lastH;
    //
    //         public void Init(double s, double g, int mh, int mw)
    //         {
    //             size = s;
    //             gap  = g;
    //             maxH = mh;
    //             maxW = mw;
    //         }
    //
    //         public bool LayoutNext(ArrangeItem item)
    //         {
    //             if (item.w + w > maxW)
    //             {
    //                 h    += lastH;
    //                 gapY++;
    //                 
    //                 if (h + h <= maxH)
    //                 {
    //                     w    = 0;
    //                     gapX = 0;
    //                 }
    //             }
    //             
    //             if (h + h > maxH)
    //             {
    //                 return false;
    //             }
    //
    //             var x = pixelX + size * w + (gapW + gapX) * gap;
    //             var y = pixelY + size * h + (gapH + gapY) * gap;
    //
    //             item.element.Arrange(new Rect(
    //                 x,
    //                 y,
    //                 item.w * size,
    //                 item.h * size));
    //             
    //             Debug.WriteLine($"{x},{y} -> {item.w}-{item.h}");
    //
    //             lastH =  item.h;
    //             w     += item.w;
    //             gapX++;
    //             return true;
    //         }
    //
    //         public void MoveRight()
    //         {
    //             pixelX += size * w;
    //             gapW   += gapX;
    //             gapX   =  0;
    //             gapY   =  0;
    //             W      += maxW;
    //             w      =  0;
    //             h      =  0;
    //         }
    //
    //         public void MoveNext()
    //         {
    //             pixelY +=  size * h;
    //             gapH   += gapY;
    //             gapX   =  0;
    //             gapY   =  0;
    //             W = 0;
    //             w      =  0;
    //             h      =  0;
    //         }
    //
    //         public int W { get; private set; }
    //     }
    //
    //     protected override Size MeasureOverride(Size availableSize)
    //     {
    //         var unitSize = ItemSize;
    //         var gap      = Gap;
    //
    //         foreach (FrameworkElement element in Children)
    //         {
    //             if (element is null)
    //             {
    //                 continue;
    //             }
    //
    //             if (element.Visibility == Visibility.Collapsed)
    //             {
    //                 continue;
    //             }
    //
    //             var h      = GetH(element);
    //             var w      = GetW(element);
    //             var width  = w * unitSize;
    //             var height = h * unitSize;
    //
    //             element.Height = height;
    //             element.Width  = width;
    //             element.Measure(new Size(height, width));
    //             availableSize.Width  += width;
    //             availableSize.Height =  Math.Max(availableSize.Height, height);
    //         }
    //
    //
    //         return base.MeasureOverride(availableSize);
    //     }
    //
    //     protected override Size ArrangeOverride(Size finalSize)
    //     {
    //         var unitSize = ItemSize;
    //         var gap      = Gap;
    //         var (items, maxH, maxW) = TakeAvailableElements();
    //         var maxLayoutColumn = ((int)(finalSize.Width - 1) / unitSize) - 1;
    //         var window          = new LayoutWindow();
    //
    //         window.Init(unitSize, gap, maxH, maxW);
    //
    //         if (window.W > maxLayoutColumn)
    //         {
    //             // 不需要布局了
    //             return finalSize;
    //         }
    //
    //         foreach (var item in items)
    //         {
    //             if (!window.LayoutNext(item))
    //             {
    //                 if (window.W + item.w > maxLayoutColumn)
    //                 {
    //                     window.MoveNext();
    //                 }
    //                 else
    //                 {
    //                     window.MoveRight();
    //                 }
    //                 window.LayoutNext(item);
    //             }
    //         }
    //
    //         return finalSize;
    //     }
    //
    //     private (ArrangeItem[], int, int) TakeAvailableElements()
    //     {
    //         var list = new List<FrameworkElement>(32);
    //         var maxH = 0;
    //         var maxW = 0;
    //         list.AddMany(Children.Cast<FrameworkElement>()
    //                               .Where(element => element is not null && element.Visibility != Visibility.Collapsed));
    //         return new ValueTuple<ArrangeItem[], int, int>(list.Select(x =>
    //         {
    //             var i = new ArrangeItem
    //             {
    //                 element = x,
    //                 w       = GetW(x),
    //                 h       = GetH(x)
    //             };
    //             maxH = Math.Max(maxH, i.h);
    //             maxW = Math.Max(maxW, i.w);
    //             return i;
    //         }).ToArray(), maxH, maxW);
    //     }
    //
    //     public double Gap
    //     {
    //         get => (double)GetValue(GapProperty);
    //         set => SetValue(GapProperty, value);
    //     }
    //     //
    //     // /// <summary>
    //     // /// 每行的最大高度数量
    //     // /// </summary>
    //     // /// <remarks>
    //     // /// <para>最大值为：16</para>
    //     // /// <para>最小值为：1</para>
    //     // /// </remarks>
    //     // public int UnitPerRow
    //     // {
    //     //     get => (int)GetValue(UnitPerRowProperty);
    //     //     set => SetValue(UnitPerRowProperty, Math.Clamp(value, 1, 16));
    //     // }
    //
    //     /// <summary>
    //     /// 每个单位的高度
    //     /// </summary>
    //     /// <remarks>
    //     /// <para>最大值为：128px</para>
    //     /// <para>最小值为：32px</para>
    //     /// </remarks>
    //     public double ItemSize
    //     {
    //         get => (double)GetValue(ItemSizeProperty);
    //         set => SetValue(ItemSizeProperty, Math.Clamp(value, 32, 256));
    //     }
    // }
}