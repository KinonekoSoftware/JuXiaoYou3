namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class UserEventMessageControl : ChatMessageBase
    {
        static UserEventMessageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserEventMessageControl), new FrameworkPropertyMetadata(typeof(UserEventMessageControl)));
        }
        
        
        public static readonly DependencyProperty CharacterNameProperty = DependencyProperty.Register(
            nameof(CharacterName),
            typeof(string),
            typeof(UserEventMessageControl),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content),
            typeof(string),
            typeof(UserEventMessageControl),
            new PropertyMetadata(default(string)));

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public string CharacterName
        {
            get => (string)GetValue(CharacterNameProperty);
            set => SetValue(CharacterNameProperty, value);
        }
    }
}