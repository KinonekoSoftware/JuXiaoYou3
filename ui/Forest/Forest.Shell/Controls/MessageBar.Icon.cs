namespace Acorisoft.FutureGL.Forest.Controls
{


    public class ImageMessageBar : MessageBar
    {
        static ImageMessageBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageMessageBar), new FrameworkPropertyMetadata(typeof(ImageMessageBar)));
        }


        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image),
            typeof(ImageSource),
            typeof(ImageMessageBar),
            new PropertyMetadata(default(ImageSource)));

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
    }
}