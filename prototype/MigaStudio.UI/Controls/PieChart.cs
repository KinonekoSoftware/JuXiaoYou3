using System.Diagnostics;
using System.Linq;
using Acorisoft.FutureGL.MigaStudio.Controls.Models;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class PieChart : PolarControl
    {
        static PieChart()
        {
            ForegroundProperty.AddOwner(typeof(PieChart))
                .OverrideMetadata(typeof(PieChart),
                    new FrameworkPropertyMetadata(OnForegroundChanged));
            
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PieChart), new FrameworkPropertyMetadata(typeof(PieChart)));
            RingSizeProperty = DependencyProperty.Register(
                nameof(RingSize),
                typeof(double),
                typeof(PieChart),
                new PropertyMetadata(Boxing.DoubleValues[8]));
            
            DataProperty = DependencyProperty.Register(
                nameof(Data),
                typeof(PieChartDataSet),
                typeof(PieChart),
                new PropertyMetadata(default(PieChartDataSet), InvalidateVisual));
        }
        protected static void InvalidateVisual(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (PieChart)d;
            radar.InvalidateVisual();
        }

        
        private Brush _foreground;
        private Pen   _pen;
        
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radar = (PieChart)d;

            radar._foreground = radar.Foreground ?? DefaultBrush;
            radar._pen        = new Pen(radar._foreground, radar.RingSize);
            radar.InvalidateVisual();
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            PushGuideline(drawingContext);

            var r = Math.Clamp(Math.Min(ActualWidth, ActualHeight), 32, 999) / 2d;
            var rSize = Math.Clamp(RingSize, 2, 64);
            
            // RenderBackground(drawingContext, r);
            
            //
            //
            RenderData(drawingContext, r, rSize);
            
            //
            //
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        private void RenderBackground(DrawingContext drawingContext, double r)
        {
            drawingContext.DrawEllipse(null, _pen, new Point(r, r), r, r);
        }
        
        private void RenderData(DrawingContext drawingContext, double r1, double rSize)
        {

            if (Data is null || Data.Count == 0)
            {
                //
                //
                // RenderBackground(drawingContext, r);
                // return;
            }
            
            var dataChart = Data is null || Data.Count == 0 ? DefaultPieData : Data;
            var total = dataChart.Sum(x => x.Value);
            var startAngle = 0;


            if (GetRAndCenterPoint(out var center, out var r))
            {
                return;
            }

            drawingContext.DrawEllipse(null, new Pen(_foreground, 2), center, r, r);

            foreach (var data in dataChart)
            {
                var angle = (int)(data.Value / total * 360);
                var startPoint = GetPoint(startAngle, r, ref center);
                var endPoint = GetPoint(angle, r, ref center);
                Debug.WriteLine($"{startAngle}-{angle}");

                drawingContext.DrawRectangle(new SolidColorBrush(Xaml.FromHex(data.Color)), null, new Rect(startPoint, new Size(6,6)));
                drawingContext.DrawRectangle(new SolidColorBrush(Xaml.FromHex(data.Color)), null, new Rect(endPoint, new Size(6,6)));

                startAngle += angle;
            }
            
        }


        public static readonly DependencyProperty RingSizeProperty;
        
        public static readonly DependencyProperty DataProperty;

        public PieChartDataSet Data
        {
            get => (PieChartDataSet)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public double RingSize
        {
            get => (double)GetValue(RingSizeProperty);
            set => SetValue(RingSizeProperty, value);
        }
    }
}