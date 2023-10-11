namespace Acorisoft.FutureGL.Forest.Controls
{


    public class GeometryMessageBar : MessageBar
    {
        static GeometryMessageBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeometryMessageBar), new FrameworkPropertyMetadata(typeof(GeometryMessageBar)));
        }
        
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
            nameof(Geometry),
            typeof(Geometry),
            typeof(GeometryMessageBar),
            new PropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty IsFilledProperty = DependencyProperty.Register(
            nameof(IsFilled),
            typeof(bool),
            typeof(GeometryMessageBar),
            new PropertyMetadata(Boxing.False));

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, Boxing.Box(value));
        }

        public Geometry Geometry
        {
            get => (Geometry)GetValue(GeometryProperty);
            set => SetValue(GeometryProperty, value);
        }
    }
}