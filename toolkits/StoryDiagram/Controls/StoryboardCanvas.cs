using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Controls.Renderers;

namespace StoryDiagram.Controls
{
    public class StoryboardCanvas : Canvas
    {
        public static readonly DependencyProperty GridlineProperty = DependencyProperty.Register(
            nameof(Gridline),
            typeof(Brush),
            typeof(StoryboardCanvas),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray), OnGridlineChanged));

        private static readonly SolidColorBrush DefaultBrush;

        static StoryboardCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StoryboardCanvas), new FrameworkPropertyMetadata(typeof(StoryboardCanvas)));
            DefaultBrush = new SolidColorBrush(Colors.Gray);
        }

        private static void OnGridlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b  = e.NewValue as Brush;
            var sc = (StoryboardCanvas)d;
            sc.CreateGridlinePen(b);
        }

        private Pen _GridlinePen;

        private void CreateGridlinePen(Brush brush)
        {
            _GridlinePen = new Pen(brush ?? DefaultBrush, 1);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (double.IsNaN(availableSize.Width) ||
                double.IsInfinity(availableSize.Width))
            {
                availableSize.Width = 0;
            }
            
            if (double.IsNaN(availableSize.Height) ||
                double.IsInfinity(availableSize.Height))
            {
                availableSize.Height = 0;
            }
            
            
            foreach (FrameworkElement element in InternalChildren)
            {
                if (element is null)
                {
                    continue;
                }

                if (element.Visibility != Visibility.Visible)
                {
                    continue;
                }
                
                element.Measure(availableSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var maxTop           = 0;
            var maxLeft          = 0;
            var lastElementWidth = 0d;
            var lastElementHeight = 0d;
            
            foreach (FrameworkElement element in InternalChildren)
            {
                if (element is null)
                {
                    continue;
                }

                if (element.Visibility != Visibility.Visible)
                {
                    continue;
                }

                var top = Math.Clamp((int)Canvas.GetTop(element), 0, int.MaxValue);
                var left = Math.Clamp((int)Canvas.GetLeft(element),0, int.MaxValue);

                if (left > maxLeft)
                {
                    maxLeft          = left;
                    lastElementWidth = element.ActualWidth;
                }

                if (top > maxTop)
                {
                    
                    maxTop            = top;
                    lastElementHeight = element.ActualHeight;
                }

                element.Arrange(new Rect(left, top, element.DesiredSize.Width, element.DesiredSize.Height));
            }

            finalSize.Width  = Math.Max(maxLeft + lastElementWidth, finalSize.Width);
            finalSize.Height = Math.Max( maxTop + lastElementHeight, finalSize.Height);
            return finalSize;
        }

        #region OnRender
        
        

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_GridlinePen is null)
            {
                CreateGridlinePen(null);
            } 
            
            drawingContext.PushGuidelineSet(Xaml.GuidelineSet);

            //
            // 绘制
            this.Normalize(out var w, out var h);
            drawingContext.DrawBackground(w, h, Background)
                          .DrawGridline(w, h, _GridlinePen, 40);
            
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        #endregion

        public Brush Gridline
        {
            get => (Brush)GetValue(GridlineProperty);
            set => SetValue(GridlineProperty, value);
        }
    }
}