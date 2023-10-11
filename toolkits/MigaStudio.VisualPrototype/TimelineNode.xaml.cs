using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio
{
    public partial class TimelineNode
    {
        public TimelineNode()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            nameof(BorderBrush),
            typeof(Brush),
            typeof(TimelineNode),
            new PropertyMetadata(default(Brush)));

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
    }
}