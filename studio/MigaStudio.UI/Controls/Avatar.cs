namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class Avatar : Control
    {
        static Avatar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Avatar), new FrameworkPropertyMetadata(typeof(Avatar)));
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(Avatar),
            new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image),
            typeof(ImageSource),
            typeof(Avatar),
            new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty AvatarNameProperty = DependencyProperty.Register(
            nameof(AvatarName),
            typeof(string),
            typeof(Avatar),
            new PropertyMetadata(default(string)));

        public string AvatarName
        {
            get => (string)GetValue(AvatarNameProperty);
            set => SetValue(AvatarNameProperty, value);
        }
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}