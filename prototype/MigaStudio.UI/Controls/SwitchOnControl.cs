using System.Windows.Controls.Primitives;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class SwitchOnControl : ToggleButton
    {
        static SwitchOnControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchOnControl), new FrameworkPropertyMetadata(typeof(SwitchOnControl)));
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(Geometry),
            typeof(SwitchOnControl),
            new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
    
    public class HighlightControl : ToggleButton
    {
        static HighlightControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightControl), new FrameworkPropertyMetadata(typeof(HighlightControl)));
        }

        public static readonly DependencyProperty OnProperty = DependencyProperty.Register(
            nameof(On),
            typeof(Geometry),
            typeof(HighlightControl),
            new PropertyMetadata(default(Geometry)));
        
        public static readonly DependencyProperty OffProperty = DependencyProperty.Register(
            nameof(Off),
            typeof(Geometry),
            typeof(HighlightControl),
            new PropertyMetadata(default(Geometry)));

        public Geometry Off
        {
            get => (Geometry)GetValue(OffProperty);
            set => SetValue(OffProperty, value);
        }

        public Geometry On
        {
            get => (Geometry)GetValue(OnProperty);
            set => SetValue(OnProperty, value);
        }
    }
}