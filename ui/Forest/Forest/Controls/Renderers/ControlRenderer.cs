using System.Diagnostics.CodeAnalysis;
using Acorisoft.FutureGL.Forest.Utils;

namespace Acorisoft.FutureGL.Forest.Controls.Renderers
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class ControlRenderer
    {
        public static void Normalize(this FrameworkElement element, out int w, out int h)
        {
            var width  = element.ActualWidth;
            var height = element.ActualHeight;

            if (double.IsNaN(width))
            {
                width = 0;
            }

            if (double.IsNaN(height))
            {
                height = 0;
            }

            w = (int)width;
            h = (int)height;
        }

        public static DrawingContext DrawGridline(this DrawingContext drawingContext, int w, int h, Pen pen, int gap)
        {
            if (pen is null)
            {
                return drawingContext;
            }

            var p1 = new Point();
            var p2 = new Point();
            gap = Math.Clamp(gap, 4, 300);

            for (var y = 0; y < h; y += gap)
            {
                p1.X = 0;
                p2.X = w;
                p1.Y = y;
                p2.Y = y;
                drawingContext.DrawLine(pen, p1, p2);
            }

            for (var x = 0; x < w; x += gap)
            {
                p1.X = x;
                p2.X = x;
                p1.Y = 0;
                p2.Y = h;
                drawingContext.DrawLine(pen, p1, p2);
            }

            return drawingContext;
        }

        public static DrawingContext DrawBackground(this DrawingContext drawingContext, int w, int h, Brush bg)
        {
            bg ??= Xaml.Transparent;

            drawingContext.DrawRectangle(bg, null, new Rect(0, 0, w, h));

            return drawingContext;
        }
    }
}