namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class SimpleButton : Button
    {
        private static readonly object DefaultCornerRadius = new CornerRadius(8);

        static SimpleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleButton),
                new FrameworkPropertyMetadata(typeof(SimpleButton)));
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

        public static readonly DependencyProperty PressForegroundBrushProperty = DependencyProperty.Register(
            nameof(PressForegroundBrush),
            typeof(Brush),
            typeof(SimpleButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PressBackgroundBrushProperty = DependencyProperty.Register(
            nameof(PressBackgroundBrush),
            typeof(Brush),
            typeof(SimpleButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(SimpleButton),
            new PropertyMetadata(DefaultCornerRadius));

        public static readonly DependencyProperty HoverForegroundBrushProperty = DependencyProperty.Register(
            nameof(HoverForegroundBrush),
            typeof(Brush),
            typeof(SimpleButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HoverBackgroundBrushProperty = DependencyProperty.Register(
            nameof(HoverBackgroundBrush),
            typeof(Brush),
            typeof(SimpleButton),
            new PropertyMetadata(null));
    }
}