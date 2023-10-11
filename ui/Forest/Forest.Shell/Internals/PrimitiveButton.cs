using System.Windows.Controls;

namespace Acorisoft.FutureGL.Forest.Internals
{
    public enum WindowButtonType
    {
        Maximum,
        Minimum,
        Close,
        GoBack
    }
    
    public class PrimitiveButton : Button
    {
        private static readonly object DefaultCornerRadius = new CornerRadius(8);

        static PrimitiveButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PrimitiveButton),
                new FrameworkPropertyMetadata(typeof(PrimitiveButton)));
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Brush HoverBackgroundBrush
        {
            get => (Brush)GetValue(HoverBackgroundBrushProperty);
            set => SetValue(HoverBackgroundBrushProperty, value);
        }

        public Brush HoverForegroundBrush
        {
            get => (Brush)GetValue(HoverForegroundBrushProperty);
            set => SetValue(HoverForegroundBrushProperty, value);
        }

        public Brush PressBackgroundBrush
        {
            get => (Brush)GetValue(PressBackgroundBrushProperty);
            set => SetValue(PressBackgroundBrushProperty, value);
        }

        public Brush PressForegroundBrush
        {
            get => (Brush)GetValue(PressForegroundBrushProperty);
            set => SetValue(PressForegroundBrushProperty, value);
        }

        public WindowState WindowState
        {
            get => (WindowState)GetValue(WindowStateProperty);
            set => SetValue(WindowStateProperty, value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            nameof(Mode),
            typeof(WindowButtonType),
            typeof(PrimitiveButton),
            new PropertyMetadata(default(WindowButtonType)));

        public WindowButtonType Mode
        {
            get => (WindowButtonType)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
               

        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register(
            nameof(WindowState),
            typeof(WindowState),
            typeof(PrimitiveButton),
            new PropertyMetadata(WindowState.Normal));
        public static readonly DependencyProperty PressForegroundBrushProperty = DependencyProperty.Register(
            nameof(PressForegroundBrush),
            typeof(Brush),
            typeof(PrimitiveButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PressBackgroundBrushProperty = DependencyProperty.Register(
            nameof(PressBackgroundBrush),
            typeof(Brush),
            typeof(PrimitiveButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(PrimitiveButton),
            new PropertyMetadata(DefaultCornerRadius));

        public static readonly DependencyProperty HoverForegroundBrushProperty = DependencyProperty.Register(
            nameof(HoverForegroundBrush),
            typeof(Brush),
            typeof(PrimitiveButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HoverBackgroundBrushProperty = DependencyProperty.Register(
            nameof(HoverBackgroundBrush),
            typeof(Brush),
            typeof(PrimitiveButton),
            new PropertyMetadata(null));
    }
}