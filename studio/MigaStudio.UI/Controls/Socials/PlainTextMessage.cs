namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class PlainTextMessage : UserMessageBase
    {
        static PlainTextMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainTextMessage), new FrameworkPropertyMetadata(typeof(PlainTextMessage)));
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content),
            typeof(string),
            typeof(PlainTextMessage),
            new PropertyMetadata(default(string)));

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        
    }
}