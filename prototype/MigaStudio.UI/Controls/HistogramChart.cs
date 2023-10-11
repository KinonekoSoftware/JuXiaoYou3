using System.Collections.Generic;
using System.Linq;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class HistogramChart : PolarControl
    {
        static HistogramChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HistogramChart), new FrameworkPropertyMetadata(typeof(HistogramChart)));

            DataProperty = DependencyProperty.Register(
                nameof(Data),
                typeof(List<int>),
                typeof(HistogramChart),
                new PropertyMetadata(default(List<int>), InvalidateVisual));

            MaximumProperty = DependencyProperty.Register(
                nameof(Maximum),
                typeof(int),
                typeof(HistogramChart),
                new PropertyMetadata(Boxing.IntValues[10]));
            ColorProperty = DependencyProperty.Register(
                nameof(Color),
                typeof(string),
                typeof(HistogramChart),
                new PropertyMetadata(default(string)));
            
            AxisProperty = DependencyProperty.Register(
                nameof(Axis),
                typeof(List<string>),
                typeof(HistogramChart),
                new PropertyMetadata(default(List<string>)));
        }


        public static readonly DependencyProperty MaximumProperty;
        public static readonly DependencyProperty ColorProperty;
        public static readonly DependencyProperty AxisProperty;

        public List<string> Axis
        {
            get => (List<string>)GetValue(AxisProperty);
            set => SetValue(AxisProperty, value);
        }
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

        protected static void InvalidateVisual(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (HistogramChart)d;
            radar.InvalidateVisual();
        }


        private void RenderingAxis(DrawingContext drawingContext)
        {
            var height = ActualHeight;
            var width = ActualWidth;
            var pen = new Pen(DefaultBrush, 1);

            RenderingAxis(drawingContext, pen, height, width, 0.0d);
            RenderingAxis(drawingContext, pen, height, width, 0.2d);
            RenderingAxis(drawingContext, pen, height, width, 0.4d);
            RenderingAxis(drawingContext, pen, height, width, 0.6d);
            RenderingAxis(drawingContext, pen, height, width, 0.8d);
        }

        private static void RenderingAxis(DrawingContext drawingContext, Pen pen, double height, double width, double percent)
        {
            var height2 = height - height * percent;
            var rect = new Rect(0, height2, width, .5d);
            drawingContext.DrawRectangle(null, pen, rect);
        }

        public HistogramChart()
        {
            this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            PushGuideline(drawingContext);

            //
            //
            RenderingAxis(drawingContext);

            //
            //
            RenderData(drawingContext);

            //
            //
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        private void RenderData(DrawingContext drawingContext)
        {
            var dataChart = Data is null || Data.Count == 0 ? DefaultHistogramData : Data;
            var max = Maximum;
            var color = Color;
            var height = ActualHeight;
            var height2 = height - 30;
            var width = ActualWidth - (dataChart.Count - 1) * 10;
            var chartWidth = (int)Math.Clamp(width / dataChart.Count, 5d, 11d);
            var x = 0d;

            foreach (var chartHeight in dataChart.Select(chart => chart * height2 / max))
            {
                drawingContext.DrawRectangle(
                    new SolidColorBrush(Xaml.FromHex(color)),
                    null,
                    new Rect(x, height - chartHeight, chartWidth, chartHeight));
                x += (chartWidth + 10);
            }
        }

        public static readonly DependencyProperty DataProperty;

        public List<int> Data
        {
            get => (List<int>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}