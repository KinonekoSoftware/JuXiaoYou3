namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class TimestampMessage : ChatMessageBase
    {
        static TimestampMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimestampMessage), new FrameworkPropertyMetadata(typeof(TimestampMessage)));
        }
        
        public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register(
            nameof(Prefix),
            typeof(string),
            typeof(TimestampMessage),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register(
            nameof(CurrentTime),
            typeof(string),
            typeof(TimestampMessage),
            new PropertyMetadata(default(string)));

        public string CurrentTime
        {
            get => (string)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }
        public string Prefix
        {
            get => (string)GetValue(PrefixProperty);
            set => SetValue(PrefixProperty, value);
        }
    }
}