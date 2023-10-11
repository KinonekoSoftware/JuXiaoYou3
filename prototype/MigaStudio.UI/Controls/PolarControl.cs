namespace  Acorisoft.FutureGL.MigaStudio.Controls
{
    public abstract class PolarControl : ChartBase
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Radius2Angle(double radius) => radius / Math.PI * 180d;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Angle2Radius(double angle) => angle / 180 * Math.PI;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetPoint(double angle, double r) => new Point { X = r * Math.Sin(Angle2Radius(angle)), Y = r * Math.Cos(Angle2Radius(angle)) };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetPoint(double angle, double r, ref Point center)
        {
            var p = new Point { X = r * Math.Sin(Angle2Radius(angle)), Y = r * Math.Cos(Angle2Radius(angle)) };
            p.X += center.X;
            p.Y += center.Y;
            return p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetAngle(double x, double y) => Math.Atan2(y, x);

        public static ArcSegment CreateArc(double startAngle, double angle, double r, double stroke)
        {
            // 0° 和 360° 是重合在一起的，ArcSegment 无法绘制重合的弧度。
            // 所以，最大角度是359。如果，目标值是 360° 则使用 PathFigure 中的属性 IsClose 闭合圆。
            
            // 根据数学公式计算出当前角度的弧度
            // 获取正弦值：a/c 
            var sin = Math.Sin(angle);
            // 获取余弦值：b/c 
            var cos = Math.Cos(angle);
            
            // 获取 a
            var a = r * sin;
            // 获取 b
            var b = r * cos;
            
            // 获取终点位置：X
            var x = a;
            
            // 获取终点位置：Y
            var y = b;
            
            // 获取终点位置
            return new ArcSegment
            {
                IsLargeArc     = angle > startAngle,
                SweepDirection = SweepDirection.Clockwise,
                Size           = new Size(r, r),
                Point          = new Point(x, y)
            };
        }
        
        protected static Geometry UpdateOpenArcGeometry(double start, double end, double r)
        {
            var pathGeometry = new PathGeometry
            {
                Figures = new PathFigureCollection()
            };

            var pathFigure = new PathFigure
            {
                IsClosed = false,
            };

            var arcSegment = new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise
            };

            var rect = new Rect(0, 0, r, r);
                
                
            pathFigure.SetValue(PathFigure.StartPointProperty, GetArcPoint(start, rect));
            pathFigure.SetValue(PathFigure.IsFilledProperty, false);
            arcSegment.SetValue(ArcSegment.PointProperty, GetArcPoint(end, rect));
            arcSegment.SetValue(ArcSegment.SizeProperty, GetArcSize(rect));
            arcSegment.SetValue(ArcSegment.IsLargeArcProperty, end - start > 180.0);
            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);
            return pathGeometry;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Size GetArcSize(Rect bound)
        {
            return new Size(bound.Width / 2.0, bound.Height / 2.0);
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static Point GetArcPoint(double degree, Rect bound)
        {
            var arcPoint = GetArcPoint(degree);
            return RelativeToAbsolutePoint(bound, arcPoint);
        }
        
        internal static Point GetArcPoint(double degree)
        {
            var num = degree * Math.PI / 180.0;
            return new Point(0.5 + 0.5 * Math.Sin(num), 0.5 - 0.5 * Math.Cos(num));
        }
        
        internal static Point RelativeToAbsolutePoint(Rect bound, Point relative)
        {
            return new Point(bound.X + relative.X * bound.Width, bound.Y + relative.Y * bound.Height);
        }


        public bool GetRAndCenterPoint(out Point center, out double r)
        {
            r = Math.Min(ActualHeight, ActualWidth) / 2d;
            center = new Point(r, r);
            return double.IsNegativeInfinity(r) || double.IsPositiveInfinity(r);
        }
    }
}