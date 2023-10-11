namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class ImageChatMessage : UserMessageBase
    {
        
        static ImageChatMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageChatMessage), new FrameworkPropertyMetadata(typeof(ImageChatMessage)));
        }
        
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image),
            typeof(ImageSource),
            typeof(ImageChatMessage),
            new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty PreviewProperty = DependencyProperty.Register(
            nameof(Preview),
            typeof(ICommand),
            typeof(ImageChatMessage),
            new PropertyMetadata(default(ICommand)));

        public ICommand Preview
        {
            get => (ICommand)GetValue(PreviewProperty);
            set => SetValue(PreviewProperty, value);
        }
        
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
    }

    public class EmojiImageMessage : UserMessageBase
    {
        static EmojiImageMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmojiImageMessage), new FrameworkPropertyMetadata(typeof(EmojiImageMessage)));
        }   
        
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image),
            typeof(ImageSource),
            typeof(EmojiImageMessage),
            new PropertyMetadata(default(ImageSource)));
        
        

        public static readonly DependencyProperty PreviewCommandProperty = DependencyProperty.Register(
            nameof(PreviewCommand),
            typeof(ICommand),
            typeof(EmojiImageMessage),
            new PropertyMetadata(default(ICommand)));

        public ICommand PreviewCommand
        {
            get => (ICommand)GetValue(PreviewCommandProperty);
            set => SetValue(PreviewCommandProperty, value);
        }

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
    }
}