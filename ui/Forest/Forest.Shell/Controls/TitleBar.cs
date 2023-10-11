using System.Windows.Controls;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class TitleBar : Control
    {
        static TitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBar), new FrameworkPropertyMetadata(typeof(TitleBar)));
        }


        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register(
            nameof(WindowState),
            typeof(WindowState),
            typeof(TitleBar),
            new PropertyMetadata(default(WindowState)));

        public WindowState WindowState
        {
            get => (WindowState)GetValue(WindowStateProperty);
            set => SetValue(WindowStateProperty, value);
        }
    }
}