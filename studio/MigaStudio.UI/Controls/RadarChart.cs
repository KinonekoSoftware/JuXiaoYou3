using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class RadarChart : PolarControl
    {
        struct AxisData
        {
            internal double[]       values;
            internal Pen             pen;
            internal SolidColorBrush bg;
        }

        private Typeface _typeface;
        private Brush    _foreground;

        static RadarChart()
        {
            ForegroundProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnForegroundChanged));

            FontSizeProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));

            FontFamilyProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));


            FontWeightProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));

            FontStretchProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));

            FontStyleProperty.AddOwner(typeof(RadarChart))
                .OverrideMetadata(typeof(RadarChart),
                    new FrameworkPropertyMetadata(OnTypefaceChanged));

            MaximumProperty = DependencyProperty.Register(
                nameof(Maximum),
                typeof(int),
                typeof(RadarChart),
                new PropertyMetadata(Boxing.IntValues[10]));
            ColorProperty = DependencyProperty.Register(
                nameof(Color),
                typeof(string),
                typeof(RadarChart),
                new PropertyMetadata(default(string)));
            
            DataProperty = DependencyProperty.Register(
                nameof(Data),
                typeof(List<int>),
                typeof(RadarChart),
                new FrameworkPropertyMetadata(default(List<int>), FrameworkPropertyMetadataOptions.AffectsRender, InvalidateVisual));

            AxisProperty = DependencyProperty.Register(
                nameof(Axis),
                typeof(List<string>),
                typeof(RadarChart),
                new FrameworkPropertyMetadata(default(List<string>), FrameworkPropertyMetadataOptions.AffectsRender, InvalidateVisual));

            ShowAxisNameProperty = DependencyProperty.Register(
                nameof(ShowAxisName),
                typeof(bool),
                typeof(RadarChart),
                new FrameworkPropertyMetadata(Boxing.True, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (RadarChart)d;

            radar._foreground = radar.Foreground ?? DefaultBrush;
            radar.InvalidateVisual();
        }

        private static void OnTypefaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (RadarChart)d;

            radar._typeface ??= new Typeface(
                radar.FontFamily,
                radar.FontStyle,
                radar.FontWeight,
                radar.FontStretch);
            radar.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (GetRAndCenterPoint(out var centerPoint, out var r))
            {
                //
                // Skip Drawing
                return;
            }


            var axes = Axis is null || Axis.Count == 0 ? new List<string>() : Axis;
            var data = Data is null || Data.Count == 0 ? new List<int>() : Data;

            /*
             *             3/2 π
             *              |
             *              |
             *      π  ----------  0
             *              |
             *              |
             *            1/2 π
             */
            //
            //
            PushGuideline(drawingContext);

            //
            //
            RenderingAxis(drawingContext, ref centerPoint, r, axes);

            //
            //
            RenderingData(drawingContext, ref centerPoint, r, axes?.Count ?? 0, data);

            //
            //
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        private void RenderingAxis(DrawingContext drawingContext, ref Point centerPoint, double r, List<string> axes)
        {
            var pen = new Pen(DefaultBrush, 1);
            if (axes is null || axes.Count == 0)
            {
                drawingContext.DrawEllipse(null, pen, centerPoint, r * 1d, r * 1d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .8d, r * .8d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .6d, r * .6d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .4d, r * .4d);
                drawingContext.DrawEllipse(null, pen, centerPoint, r * .2d, r * .2d);
            }
            else
            {
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * 1d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .8d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .6d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .4d, axes.Count, ref centerPoint));
                drawingContext.DrawGeometry(null, pen, GetPolygon(r * .2d, axes.Count, ref centerPoint));
                RenderingAxisLine(drawingContext, pen, r, axes, ref centerPoint);
            }
        }

        private void RenderingData(DrawingContext drawingContext, ref Point centerPoint, double r, int axisCount, List<int> dataSet)
        {

            var count = Math.Min(axisCount, dataSet.Count);
            var c = Color;
            var max = Maximum;

            for (var i = 0; i < count; i++)
            {
                var color = Xaml.FromHex(c);
                var axisData = new AxisData
                {
                    values = dataSet.Select(x => Math.Clamp((double)x / max, 0, 1)).ToArray(),
                    pen    = new Pen(new SolidColorBrush(color), 1),
                    bg = new SolidColorBrush(color)
                    {
                        Opacity = .1f
                    }
                };

                var geometry = GetPolygon(r, ref axisData.values, ref centerPoint);
                drawingContext.DrawGeometry(axisData.bg, axisData.pen, geometry);
            }
        }

        private void RenderingAxisLine(DrawingContext drawingContext, Pen pen, double r, List<string> axes, ref Point centerPoint)
        {
            var count = axes.Count;
            var angle = 0;
            var avg = 360 / count;
            var dpi = VisualTreeHelper.GetDpi(this);
            var showText = ShowAxisName;
            var flowDirection = FlowDirection;
            var culture = CultureInfo.CurrentCulture;
            var fontSize = Math.Clamp(FontSize, 12, 100);
            var ns = new NumberSubstitution();

            _typeface   ??= new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            _foreground ??= new SolidColorBrush(Colors.Gray);

            for (var i = 0; i < count; i++)
            {
                var axis = axes[i];
                var text = new FormattedText(
                    axis,
                    culture,
                    flowDirection,
                    _typeface,
                    fontSize,
                    _foreground,
                    ns,
                    TextFormattingMode.Display,
                    dpi.DpiScaleX * 96);

                drawingContext.DrawLine(pen, centerPoint, GetPoint(angle, r, ref centerPoint));

                // 绘制文字
                if (showText)
                    drawingContext.DrawText(text, GetPoint(angle, r, ref centerPoint));
                angle += avg;
            }
        }

        private static PathGeometry GetPolygon(double r, int count, ref Point centerPoint)
        {
            var points = new Point[count];
            var angle = 0;
            var avg = 360 / count;

            for (var i = 0; i < count; i++)
            {
                points[i] =  GetPoint(angle, r, ref centerPoint);
                angle     += avg;
            }

            var figure = new PathFigure(points[0], points.Skip(1).Select(x => new LineSegment(x, true)), true);
            return new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    figure
                }
            };
        }

        private static PathGeometry GetPolygon(double r, ref double[] values, ref Point centerPoint)
        {
            var count = values.Length;
            var points = new Point[count];
            var angle = 0;
            var avg = 360 / count;

            for (var i = 0; i < count; i++)
            {
                points[i] =  GetPoint(angle, r * values[i], ref centerPoint);
                angle     += avg;
            }

            var figure = new PathFigure(points[0], points.Skip(1).Select(x => new LineSegment(x, true)), true);
            return new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    figure
                }
            };
        }
        
        
        protected static void InvalidateVisual(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (RadarChart)d;
            radar.InvalidateVisual();
        }


        public static readonly DependencyProperty AxisProperty;
        public static readonly DependencyProperty DataProperty;
        public static readonly DependencyProperty ShowAxisNameProperty;
        public static readonly DependencyProperty MaximumProperty;
        public static readonly DependencyProperty ColorProperty;

        public string Color
        {
            get => (string)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public bool ShowAxisName
        {
            get => (bool)GetValue(ShowAxisNameProperty);
            set => SetValue(ShowAxisNameProperty, value);
        }

        public List<int> Data
        {
            get => (List<int>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public List<string> Axis
        {
            get => (List<string>)GetValue(AxisProperty);
            set => SetValue(AxisProperty, value);
        }
    }
    
}