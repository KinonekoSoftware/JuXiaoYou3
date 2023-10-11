using System.Windows.Controls;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public abstract class MessageBar : Control
    {
        public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register(
            nameof(Highlight),
            typeof(Brush),
            typeof(MessageBar),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x6f,0xa2,0x40))));


        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(MessageBar),
            new PropertyMetadata(default(string)));
        
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public Brush Highlight
        {
            get => (Brush)GetValue(HighlightProperty);
            set => SetValue(HighlightProperty, value);
        }
    }
}